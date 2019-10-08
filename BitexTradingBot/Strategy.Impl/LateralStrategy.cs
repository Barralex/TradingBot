using BitexTradingBot.Core.Constants;
using BitexTradingBot.Core.DataAccess.DataBase.Contexts;
using BitexTradingBot.Core.DataAccess.DataBase.Enums;
using BitexTradingBot.Core.DataAccess.DataBase.Models;
using BitexTradingBot.Core.Helpers;
using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using BitexTradingBot.Core.Models.Requests;
using BitexTradingBot.Core.Models.Responses.Market;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BitexTradingBot.Program;

namespace BitexTradingBot
{
    public class LateralStrategy : IStrategy
    {
        private readonly ITradingApi _tradingApi;
        private readonly IWebJobConfiguration _webJobConfiguration;
        private readonly BitexTradingBotContext _context;
        private readonly double minimumProfitPercent = 0.0125;
        private readonly double maximumProfitPercent = 0.0150;
        private readonly double percentToleranceMargin = 2;

        public LateralStrategy(ServiceResolver serviceAccessor, IWebJobConfiguration webJobConfiguration, BitexTradingBotContext context)
        {
            _tradingApi = serviceAccessor(ServiceKeyEnum.Bitex);
            _webJobConfiguration = webJobConfiguration;
            _context = context;
        }

        public IWebJobConfiguration _WebJobConfiguration { get; }

        public async Task Start()
        {
            var activeTrading = _context.Trading.Include(x => x.TradingTransactions).Where(s => s.IsActive).FirstOrDefault();

            if (activeTrading == null)
            {
                await CreateTradingOrder();
            }
            else
            {
                var order = activeTrading.TradingTransactions.Last();

                if (order.OrderTypeId == (int)OrderTypeEnum.Bid)
                {
                    await HandleBidOrder(activeTrading, order);
                }

                await HandleAskOrder(activeTrading, order);
            }
        }

        private async Task HandleAskOrder(Trading activeTrading, TradingTransaction order)
        {
            if (order.OrderStatusId == (int)OrderStatusEnum.Open)
            {
                var result = await _tradingApi.GetOrder<TradingOrder>(TradingContants.Bids, order.ExchangeOperationId);

                if (result.Details.Attributes.Status == TradingContants.OpenStatus)
                {
                    var actualBtcInformation = await _tradingApi.GetBtcPrice<Cryptocurrency>();

                    if (order.CryptocurrencyPrice.CalculatePercentDiference(actualBtcInformation.Details.Price) > percentToleranceMargin)
                    {
                        await CancelAndUpdateOrder(activeTrading, OrderStatusEnum.CanceledByPriceChange);
                    }
                }
            }
        }

        private async Task HandleBidOrder(Trading activeTrading, TradingTransaction order)
        {
            if (order.OrderStatusId == (int)OrderStatusEnum.Open)
            {
                var result = await _tradingApi.GetOrder<TradingOrder>(TradingContants.Bids, order.ExchangeOperationId);

                if (result.Details.Attributes.Status == TradingContants.OpenStatus)
                {
                    var actualBtcInformation = await _tradingApi.GetBtcPrice<Cryptocurrency>();

                    if (order.CryptocurrencyPrice.CalculatePercentDiference(actualBtcInformation.Details.Price) > percentToleranceMargin)
                    {
                        await CancelAndUpdateOrder(activeTrading, OrderStatusEnum.CanceledByPriceChange);
                    }
                }

                if (result.Details.Attributes.Status == TradingContants.CancelledStatus)
                {
                    await CancelAndUpdateOrder(activeTrading, OrderStatusEnum.ManuallyCanceled);
                }
            }
            else
            {
                await CreateTradingOrder();
            }
        }

        private async Task CancelAndUpdateOrder(Trading activeTrading, OrderStatusEnum cancellationReason)
        {
            await _tradingApi.CancelOrder(activeTrading.TradingTransactions.Last().ExchangeOperationId, TradingContants.Bids);

            activeTrading.TradingTransactions.Last().OrderStatusId = (int)cancellationReason;

            _context.Trading.Update(activeTrading);
            await _context.SaveChangesAsync();

            await CreateTradingOrder(activeTrading);
        }

        private async Task<TradingOrdenRequest> PrepareBidExchangeOrder()
        {
            var usdBalance = await _tradingApi.GetCashWallet<CashWallet>("usd");

            if (usdBalance.Attributes.Available < 1)
            {
                throw new Exception("There is not more money left");
            }

            var actualBtcInformation = await _tradingApi.GetBtcPrice<Cryptocurrency>();

            var BidPrice = ((actualBtcInformation.Details.Price) - 6500).CalculateProfitPrice(minimumProfitPercent, maximumProfitPercent);

            return new TradingOrdenRequest(usdBalance.Attributes.Available, BidPrice, _webJobConfiguration.BitexDefaultMarket, TradingContants.Bids);
        }

        private async Task CreateTradingOrder(Trading actualOrder = null)
        {
            var order = await PrepareBidExchangeOrder();

            var attemptOrderData = (await _tradingApi.PlaceOrder<TradingOrder>(order, TradingContants.Bids)).Details;

            var individualTradingTransaction = new TradingTransaction
            {
                OrderTypeId = (int)OrderTypeEnum.Bid,
                Amount = attemptOrderData.Attributes.Amount.Value,
                CryptocurrencyPrice = attemptOrderData.Attributes.Price.Value,
                ExpectedOperationResult = (attemptOrderData.Attributes.Amount.Value * attemptOrderData.Attributes.Price.Value),
                StarDate = DateTime.Now,
                OrderStatusId = (int)OrderStatusEnum.Open,
                ExchangeOperationId = attemptOrderData.Id,
            };

            if (actualOrder == null)
            {
                var tradingObject = new Trading
                {
                    StarDate = DateTime.Now,
                    IsActive = true,
                    InitialBalance = attemptOrderData.Attributes.Amount.Value,
                    TradingTransactions = new List<TradingTransaction>
                    {
                         individualTradingTransaction
                    }
                };

                _context.Trading.Add(tradingObject);
            }
            else
            {
                actualOrder.TradingTransactions.Add(individualTradingTransaction);
                _context.Trading.Update(actualOrder);
            }

            await _context.SaveChangesAsync();
        }
    }
}