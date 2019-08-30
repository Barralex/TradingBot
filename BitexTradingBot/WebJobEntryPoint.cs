using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BitexTradingBot
{
    public class WebJobEntryPoint
    {
        private readonly IWebJobConfiguration _webJobConfiguration;
        private readonly IHttpClientApi _httpClientFactory;

        public WebJobEntryPoint(IWebJobConfiguration webJobConfiguration, IHttpClientApi httpClientFactory)
        {
            _webJobConfiguration = webJobConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Run()
        {
            Console.WriteLine(_webJobConfiguration.Message);

            var result = await _httpClientFactory.GetAsync<Market>("markets/btc_usd", "bitex");

            Console.Write("El mejor bid ahora es: {0} y el mejor ask es: {1}", result.Aks.FirstOrDefault().Id, result.Bids.FirstOrDefault().Id);

        }
    }
}
