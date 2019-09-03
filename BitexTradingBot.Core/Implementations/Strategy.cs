using System;
using System.Linq;
using System.Threading.Tasks;
using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using BitexTradingBot.Core.Models.Requests;

namespace BitexTradingBot.Core.Implementations
{
    public class Strategy : IStrategy
    {
        private readonly ITradingApi _tradingApi;
        private readonly IWebJobConfiguration _webJobConfiguration;

        private readonly double minimumProfitPercent = 0.75;
        private readonly double maximumProfitPercent = 1.25;

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

                // There's nothing here. Time to place a bid. 
                // But first. I need to know the bitcoin price in order to calculate the bid price.

                var tickersResult = await _tradingApi.GetTickers<TickersRoot>();
                var btcTicker = tickersResult.Tickers.Where(x => x.Id == _webJobConfiguration.BitexDefaultMarket).FirstOrDefault();

                var actualBtcPrice = btcTicker.Attributes.Last.Value;

                // Cool, once i have the current exchage btc price. I need to calcule the bid price. Let's look for the profits and make an average.

                var ProfitAverage = GetRandomNumber(minimumProfitPercent, maximumProfitPercent);
                var BidPrice = (actualBtcPrice * ProfitAverage) + actualBtcPrice;

                // Ok, I have average. Time To make the bid

                //var bidRequest = new TradingOrdenRequest(BidPrice);

                //_tradingApi.PlaceOrder<TradingOrder>()

            }

        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

    }
}
