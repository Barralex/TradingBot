using BitexTradingBot.Core.DataAccess.DataInvoke.ApiClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitexTradingBot.Core.DataAccess.DataInvoke
{
    public class HttpClientApi : IHttpClientApi
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientApi(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<TResponse> InvokeService<TResponse>(ApiClientOptions options) where TResponse : class
        {
            var result = await InvokeBaseService(options);
            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        public async Task InvokeService(ApiClientOptions options)
        {
            await InvokeBaseService(options);
        }

        private async Task<string> InvokeBaseService(ApiClientOptions options)
        {
            using (HttpClient client = _clientFactory.CreateClient(options.HttlClientName))
            {
                HttpResponseMessage response = null;
                HttpContent httpContent = null;

                if (options.ContentType == ApiClientRequestContentTypes.Json)
                {
                    DefaultContractResolver contractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };

                    var requestObject = JsonConvert.SerializeObject(options.RequestContent, new JsonSerializerSettings
                    {
                        ContractResolver = contractResolver,
                        Formatting = Formatting.Indented
                    });

                    httpContent = new StringContent(requestObject, Encoding.UTF8, "application/json");
                }
                else
                {
                    throw new NotImplementedException("HttpClientApi only accept Json as contentType");
                }

                if (options.RequestType == ApiClientRequestTypes.Get) response = await client.GetAsync(options.Uri);
                if (options.RequestType == ApiClientRequestTypes.Post) response = await client.PostAsync(options.Uri, httpContent);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}