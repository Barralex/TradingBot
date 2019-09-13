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

namespace BitexTradingBot
{
    public class Strategy : IStrategy
    {
        private readonly BitexTradingBotContext _context;
        private readonly ITradingApi _tradingApi;
        private readonly IWebJobConfiguration _webJobConfiguration;
        private readonly double maximumProfitPercent = 0.0130;
        private readonly double minimumProfitPercent = 0.01;
        private readonly double percentToleranceMargin = 2;

        public Strategy(ITradingApi tradingApi, IWebJobConfiguration webJobConfiguration, BitexTradingBotContext context)
        {
            _tradingApi = tradingApi;
            _webJobConfiguration = webJobConfiguration;
            _context = context;
        }

        public IWebJobConfiguration _WebJobConfiguration { get; }

        public async Task Start()
        {
            var activeTrading = _context.Trading.Include(x => x.TradingTransactions).Where(s => s.IsActive).FirstOrDefault();

            if (activeTrading == null)
            {
                await SetTradingOrder(TradingContants.Bids);
            }
            else
            {
                var order = activeTrading.TradingTransactions.Last();

                if (order.OrderTypeId == (int)OrderTypeEnum.Bid)
                {
                    await HandleBidOrder(activeTrading, order);
                }
                else
                {
                    await HandleAskOrder(activeTrading, order);
                }
            }
        }

        private async Task CancelAndUpdateOrder(Trading activeTrading, OrderStatusEnum cancellationReason)
        {
            await _tradingApi.CancelOrder(activeTrading.TradingTransactions.Last().ExchangeOperationId, TradingContants.Bids);

            activeTrading.TradingTransactions.Last().OrderStatusId = (int)cancellationReason;
            _context.Trading.Update(activeTrading);
            await _context.SaveChangesAsync();

            await SetTradingOrder(TradingContants.Bids, activeTrading);
        }

        private async Task HandleAskOrder(Trading activeTrading, TradingTransaction order)
        {
            if (order.OrderStatusId == (int)OrderStatusEnum.Open)
            {
                var orderStatus = (await _tradingApi.GetOrder<TradingOrder>(TradingContants.Aks, order.ExchangeOperationId))
                    .Details.Attributes.Status;

                if (orderStatus != TradingContants.OpenStatus)
                {
                    activeTrading.TradingTransactions.Last().OrderStatusId = (int)orderStatus.BitexStatusToDatabase();
                    _context.Trading.Update(activeTrading);
                    await _context.SaveChangesAsync();

                    await SetTradingOrder(TradingContants.Bids);
                }
            }
        }

        private async Task HandleBidOrder(Trading activeTrading, TradingTransaction order)
        {
            if (order.OrderStatusId == (int)OrderStatusEnum.Open)
            {
                var result = (await _tradingApi.GetOrder<TradingOrder>(TradingContants.Bids, order.ExchangeOperationId))
                    .Details.Attributes;

                if (result.Status == TradingContants.OpenStatus)
                {
                    var actualBtcPrice = (await _tradingApi.GetBtcPrice<Cryptocurrency>()).Details.Price;

                    if (order.CryptocurrencyPrice.CalculatePercentDiference(actualBtcPrice) > percentToleranceMargin)
                    {
                        await CancelAndUpdateOrder(activeTrading, OrderStatusEnum.CanceledByPriceChange);
                    }
                }

                if (result.Status == TradingContants.CancelledStatus)
                {
                    await CancelAndUpdateOrder(activeTrading, OrderStatusEnum.ManuallyCanceled);
                }

                if (result.Status == TradingContants.Finished)
                {
                    await SetTradingOrder(TradingContants.Aks);
                }
            }
            else if (order.OrderStatusId == (int)OrderStatusEnum.Finished)
            {
                await SetTradingOrder(TradingContants.Aks);
            }
            else
            {
                await SetTradingOrder(TradingContants.Bids);
            }
        }

        private async Task<TradingOrdenRequest> PrepareAskOrder(double lastBidPrice)
        {
            var btcBalance = await _tradingApi.GetCoinWallet<Wallet>(_webJobConfiguration.BtcWalletId);

            if (btcBalance.Attributes.Available < 1)
            {
                throw new Exception("There is not more btc left");
            }

            var PriceMargin = lastBidPrice.CalculateProfitMargin(minimumProfitPercent, maximumProfitPercent);

            return new TradingOrdenRequest(btcBalance.Attributes.Available, lastBidPrice + PriceMargin,
                _webJobConfiguration.BitexDefaultMarket, TradingContants.Aks);
        }

        private async Task<TradingOrdenRequest> PrepareBidOrder()
        {
            var usdBalance = await _tradingApi.GetCashWallet<Wallet>("usd");

            if (usdBalance.Attributes.Available < 1)
            {
                throw new Exception("There is not more money left");
            }

            var actualBtcInformation = await _tradingApi.GetBtcPrice<Cryptocurrency>();

            var PriceMargin = ((actualBtcInformation.Details.Price) - 6500)
                .CalculateProfitMargin(minimumProfitPercent, maximumProfitPercent);

            return new TradingOrdenRequest(usdBalance.Attributes.Available, ((actualBtcInformation.Details.Price) - 6500) - PriceMargin,
            _webJobConfiguration.BitexDefaultMarket, TradingContants.Bids);
        }

        private async Task SetTradingOrder(string orderType, Trading actualOrder = null)
        {
            TradingOrdenRequest order;

            if (orderType == TradingContants.Bids)
            {
                order = await PrepareBidOrder();
            }
            else
            {
                order = await PrepareAskOrder(actualOrder.TradingTransactions.Last().CryptocurrencyPrice);
            }

            var attemptOrderData = (await _tradingApi.PlaceOrder<TradingOrder>(order, orderType)).Details;

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