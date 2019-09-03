using BitexTradingBot.Core.Constants;
using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using BitexTradingBot.Core.Models.Requests;
using System.Collections;
using System.Collections.Generic;
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


            //var request = new TradingOrdenRequest(10, 1, _webJobConfiguration.BitexDefaultMarket, TradingContants.Bids);

            //var result = await _tradingApi.PlaceOrder<TradingOrder>(request, TradingContants.Bids);

            ////var a = await _tradingApi.GetOwnOrders<Orders>();
            ///
            var a = await _tradingApi.GetCashWallet<CashWallet>("usd");

        }
    }
}
