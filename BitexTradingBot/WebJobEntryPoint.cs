using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
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
            Console.WriteLine(_webJobConfiguration.Message);

            var result = await _tradingApi.GetTickers<Tickers>();

            //Console.Write("El mejor bid ahora es: {0} y el mejor ask es: {1}", result.Aks.FirstOrDefault().Id, result.Bids.FirstOrDefault().Id);

        }
    }
}
