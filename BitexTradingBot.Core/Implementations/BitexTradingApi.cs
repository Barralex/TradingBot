using BitexTradingBot.Core.DataAccess.DataInvoke;
using BitexTradingBot.Core.DataAccess.DataInvoke.ApiClient;
using BitexTradingBot.Core.Interfaces;
using System.Threading.Tasks;

namespace BitexTradingBot.Core.Implementations
{
    public class BitexTradingApi : ITradingApi
    {
        private readonly IHttpClientApi _httpClientApi;

        public BitexTradingApi(IHttpClientApi httpClientApi)
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

        public async Task<TResponse> GetOwnOrders<TResponse>() where TResponse : class
        {
            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = "orders",
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Get
            });
        }

        public async Task<TResponse> PlaceOrder<TResponse>(object request, string orderType) where TResponse : class
        {
            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = orderType,
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Post,
                RequestContent = request
            });
        }

        public async Task<TResponse> GetOrder<TResponse>(string orderType, string orderId) where TResponse : class
        {
            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = $"{orderType}/{orderId}",
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Get
            });
        }

        public async Task CancelOrder(string orderId, string orderType)
        {
            await _httpClientApi.InvokeService(new ApiClientOptions
            {
                Uri = $"{orderType }/{orderId}/cancel",
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Post,
                RequestContent = ""
            });
        }

        public async Task<TResponse> GetCashWallet<TResponse>(string currency) where TResponse : class
        {
            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = $"cash_wallets/{currency}",
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Get
            });
        }

        public async Task<TResponse> GetCoinWallet<TResponse>(int walletId) where TResponse : class
        {
            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = $"coin_wallets/{walletId}",
                HttlClientName = "bitex",
                RequestType = ApiClientRequestTypes.Get
            });
        }

        public async Task<TResponse> GetBtcPrice<TResponse>() where TResponse : class
        {
            return await _httpClientApi.InvokeService<TResponse>(new ApiClientOptions
            {
                Uri = "cryptocurrency/quotes/latest?id=1",
                HttlClientName = "coinmarketcap",
                RequestType = ApiClientRequestTypes.Get
            });
        }
    }
}