using System;
using System.Linq;
using System.Threading.Tasks;
using BitexTradingBot.Core.Constants;
using BitexTradingBot.Core.Helpers;
using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using BitexTradingBot.Core.Models.Requests;
using BitexTradingBot.Core.Models.Responses.Market;

namespace BitexTradingBot
{
    public class Strategy : IStrategy
    {
        private readonly ITradingApi _tradingApi;
        private readonly IWebJobConfiguration _webJobConfiguration;

        private readonly double minimumProfitPercent = 0.0075;
        private readonly double maximumProfitPercent = 0.01;

        public Strategy(ITradingApi tradingApi, IWebJobConfiguration webJobConfiguration)
        {
            _tradingApi = tradingApi;
            _webJobConfiguration = webJobConfiguration;
        }

        public IWebJobConfiguration _WebJobConfiguration { get; }

        public async Task Start()
        {

            // Look for my actual orders

            var result = await _tradingApi.GetOwnOrders<OrdersRoot>();

            if (result.Orders.Count == 0)
            {
                var attemptOrder = await _tradingApi.PlaceOrder<TradingOrder>(PrepareExchangeOrder(TradingContants.Bids), TradingContants.Bids);
            }
            else
            {

                // What kind of order do I have

                var order = result.Orders.FirstOrDefault();

                if (order.Type.IsOrderType(TradingContants.Bids))
                {

                    // Ok, what do we have here. It's a bid. We need to know if is old enough to make a new one.
           


                }


            }



        }

        private async Task<TradingOrdenRequest> PrepareExchangeOrder(string orderType)
        {
            var usdBalance = await _tradingApi.GetCashWallet<CashWallet>("usd");

            var tickersResult = await _tradingApi.GetTickers<TickersRoot>();
            var btcTicker = tickersResult.Tickers.Where(x => x.Id == _webJobConfiguration.BitexDefaultMarket).FirstOrDefault();

            var actualBtcInformation = await _tradingApi.GetBtcPrice<Cryptocurrency>();

            var BidPrice = /*actualBtcInformation.Details.Price*/4100.20.CalculateOrderPrice(minimumProfitPercent, maximumProfitPercent);

            if (usdBalance.Attributes.Available == 0)
            {
                throw new Exception("There is not more money left");
            }

            return new TradingOrdenRequest(usdBalance.Attributes.Available, BidPrice, _webJobConfiguration.BitexDefaultMarket, orderType);
        }

    }
}
