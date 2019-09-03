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
                var usdBalance = await _tradingApi.GetCashWallet<CashWallet>("usd");

                // There's nothing here. Time to place a bid. 
                // But first. I need to know the bitcoin price in order to calculate the bid price.

                var tickersResult = await _tradingApi.GetTickers<TickersRoot>();
                var btcTicker = tickersResult.Tickers.Where(x => x.Id == _webJobConfiguration.BitexDefaultMarket).FirstOrDefault();

                var actualBtcInformation= await _tradingApi.GetBtcPrice<Cryptocurrency>(); /*4100.20;*/

                // Cool, once i have the current exchage btc price. I need to calcule the bid price. Let's look for the profits and make an average.

                var BidPrice = actualBtcInformation.Details.Price.CalculateOrderPrice(minimumProfitPercent, maximumProfitPercent);

                // Ok, I have average. Time To make the bid

                var bidRequest = new TradingOrdenRequest(usdBalance.Attributes.Available, BidPrice, _webJobConfiguration.BitexDefaultMarket, TradingContants.Bids);

                var attemptOrder = _tradingApi.PlaceOrder<TradingOrder>(bidRequest, TradingContants.Bids);

            }
            else
            {

                // What kind of order do I have

                var order = result.Orders.FirstOrDefault();

                if(order.Type == "bids")
                {

                    // Ok, what do we have here. It's a bid. We need to know if is old enough to make a new one.



                }


            }



        }

    }
}
