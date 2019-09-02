using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using BitexTradingBot.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitexTradingBot
{
    public class WebJobEntryPoint
    {
        private readonly IWebJobConfiguration _webJobConfiguration;
        private readonly ITradingApi _tradingApi;

        public WebJobEntryPoint(IWebJobConfiguration webJobConfiguration, ITradingApi tradingApi)
        {
            _webJobConfiguration = webJobConfiguration;
            _tradingApi = tradingApi;
        }

        public async Task Run()
        {


            var request = new BidRequest(10, 1, _webJobConfiguration.BitexDefaultMarket, "bids");

            var result = await _tradingApi.PlaceBidOrder<Bid>(request);



        }
    }
}
