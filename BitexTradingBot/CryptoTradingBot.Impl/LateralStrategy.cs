using BitexTradingBot.Core.Constants;
using BitexTradingBot.Core.DataAccess.DataBase.Enums;
using BitexTradingBot.Core.DataAccess.DataBase.Models;
using BitexTradingBot.Core.Helpers;
using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using BitexTradingBot.Core.Models.Requests;
using BitexTradingBot.Core.Models.Responses.Market;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BitexTradingBot
{
    public class LateralStrategy : IStrategy
    {
        private readonly IStrategyDatabase _strategyDatabaseAccess;
        private readonly ITradingApi _tradingApi;
        private readonly IWebJobConfiguration _webJobConfiguration;
        private readonly double maximumProfitPercent = 0.0130;
        private readonly double minimumProfitPercent = 0.01;
        private readonly double percentToleranceMargin = 2;

        public LateralStrategy(ITradingApi tradingApi, IWebJobConfiguration webJobConfiguration, IStrategyDatabase databaseAccess)
        {
            _tradingApi = tradingApi;
            _webJobConfiguration = webJobConfiguration;
            _strategyDatabaseAccess = databaseAccess;
        }

        public IWebJobConfiguration _WebJobConfiguration { get; }

        public async Task Start()
        {
            var activeTrading = _strategyDatabaseAccess.GetLastTransaction();

            if (activeTrading == null)
            {
                await SetBidOrder();
            }
            else
            {
                var lastOrder = activeTrading.TradingTransactions.Last();

                if (lastOrder.OrderTypeId == (int)OrderTypeEnum.Bid)
                {
                    await ApplyBidStrategy(activeTrading, lastOrder);
                }
                else
                {
                    await ApplyAskStrategy(activeTrading, lastOrder);
                }
            }
        }

        private async Task ApplyAskStrategy(Trading activeTrading, TradingTransaction order)
        {
            if (order.OrderStatusId == (int)OrderStatusEnum.Open)
            {
                var orderStatus = (await _tradingApi.GetOrder<TradingOrder>(TradingContants.Aks, order.ExchangeOperationId))
                    .Details.Attributes.Status;

                if (orderStatus != TradingContants.OpenStatus)
                {
                    await _strategyDatabaseAccess.UpdateOrderStatus(activeTrading, orderStatus.BitexStatusToDatabase());
                    await SetAskOrder(activeTrading);
                }
            }
        }

        private async Task ApplyBidStrategy(Trading activeTrading, TradingTransaction order)
        {
            if (order.OrderStatusId == (int)OrderStatusEnum.Open)
            {
                var result = (await _tradingApi.GetOrder<TradingOrder>(TradingContants.Bids, order.ExchangeOperationId))
                    .Details.Attributes;

                if (result.Status == TradingContants.OpenStatus)
                {
                    var actualBtcPrice = (await _tradingApi.GetBtcPrice<Cryptocurrency>()).Details.Price;

                    if (order.CryptocurrencyPrice.CalculatePercentDiference(actualBtcPrice) >= percentToleranceMargin)
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
                    await _strategyDatabaseAccess.UpdateOrderStatus(activeTrading, OrderStatusEnum.Finished);
                    await SetAskOrder(activeTrading);
                }
            }
        }

        private async Task CancelAndUpdateOrder(Trading activeTrading, OrderStatusEnum statusChangeReason)
        {
            await _tradingApi.CancelOrder(activeTrading.TradingTransactions.Last().ExchangeOperationId, TradingContants.Bids);

            await _strategyDatabaseAccess.UpdateOrderStatus(activeTrading, statusChangeReason);

            await SetBidOrder(activeTrading);
        }

        private async Task SetAskOrder(Trading actualOrder)
        {
            var btcBalance = await _tradingApi.GetCoinWallet<Wallet>(_webJobConfiguration.BtcWalletId);

            if (btcBalance.Attributes.Available < 1)
            {
                throw new Exception("There is not more btc left");
            }

            var lastBidPrice = actualOrder.TradingTransactions.Last().CryptocurrencyPrice;

            var PriceMargin = lastBidPrice.CalculateProfitMargin(minimumProfitPercent, maximumProfitPercent);

            var order = new TradingOrdenRequest(btcBalance.Attributes.Available, lastBidPrice + PriceMargin,
                _webJobConfiguration.BitexDefaultMarket, TradingContants.Aks);

            var attemptOrderData = (await _tradingApi.PlaceOrder<TradingOrder>(order, TradingContants.Aks)).Details;

            await _strategyDatabaseAccess.UpdateTradingOrder(attemptOrderData, actualOrder);
        }

        private async Task SetBidOrder(Trading activeTrading = null)
        {
            var usdBalance = await _tradingApi.GetCashWallet<Wallet>("usd");

            if (usdBalance.Attributes.Available < 1)
            {
                throw new Exception("There is not more money left");
            }

            var actualBtcInformation = await _tradingApi.GetBtcPrice<Cryptocurrency>();

            var PriceMargin = ((actualBtcInformation.Details.Price) - 6500)
                .CalculateProfitMargin(minimumProfitPercent, maximumProfitPercent);

            var order = new TradingOrdenRequest(usdBalance.Attributes.Available, ((actualBtcInformation.Details.Price) - 6500) - PriceMargin,
            _webJobConfiguration.BitexDefaultMarket, TradingContants.Bids);

            var attemptOrderData = (await _tradingApi.PlaceOrder<TradingOrder>(order, TradingContants.Bids)).Details;

            if (activeTrading == null)
            {
                await _strategyDatabaseAccess.CreateTradingOrder(attemptOrderData);
            }
            else
            {
                await _strategyDatabaseAccess.UpdateTradingOrder(attemptOrderData, activeTrading);
            }
        }
    }
}