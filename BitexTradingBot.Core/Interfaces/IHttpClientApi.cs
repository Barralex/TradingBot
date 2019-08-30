using System.Threading.Tasks;

namespace BitexTradingBot.Core.Interfaces
{
    public interface IHttpClientApi
    {
        Task<TResponse> GetAsync<TResponse>(string uri, string httpClient) where TResponse : class;
    }
}
