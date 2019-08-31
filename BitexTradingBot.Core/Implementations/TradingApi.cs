using System.Threading.Tasks;
using BitexTradingBot.Core.Interfaces;

namespace BitexTradingBot.Core.Implementations
{
    public class TradingApi : ITradingApi
    {
        private readonly IHttpClientApi _httpClientApi;

        public TradingApi(IHttpClientApi httpClientApi)
        {
            _httpClientApi = httpClientApi;
        }

        public async Task<TResponse> GetTickers<TResponse>() where TResponse : class
        {
            return await _httpClientApi.GetAsync<TResponse>("tickers", "bitex");
        }
    }
}
