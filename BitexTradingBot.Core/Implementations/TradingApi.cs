using System.Threading.Tasks;
using BitexTradingBot.Core.DataAccess.DataInvoke;
using BitexTradingBot.Core.DataAccess.DataInvoke.ApiClient;
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
            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = "tickers",
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Get
            });
        }

        public async Task<TResponse> PlaceBidOrder<TResponse>(object request) where TResponse : class
        {

            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = "bids",
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Post,
                RequestContent = request
            });
        }

        public async Task<TResponse> CancelBidOrder<TResponse>(int bidId) where TResponse : class
        {
            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = $"bids/{bidId}/cancel",
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Post,
                RequestContent = ""
            });
        }

    }
}
