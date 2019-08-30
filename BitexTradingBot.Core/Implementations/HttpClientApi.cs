using System.Net.Http;
using System.Threading.Tasks;
using BitexTradingBot.Core.Interfaces;
using Newtonsoft.Json;

namespace BitexTradingBot.Core.Implementations
{
    public class HttpClientApi : IHttpClientApi
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientApi(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<TResponse> GetAsync<TResponse>(string uri, string httpClient) where TResponse : class
        {

            using (HttpClient client = _clientFactory.CreateClient(httpClient))
            {
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseBody);
            }

        }
    }
}
