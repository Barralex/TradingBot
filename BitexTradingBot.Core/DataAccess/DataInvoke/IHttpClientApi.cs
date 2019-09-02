using BitexTradingBot.Core.DataAccess.DataInvoke.ApiClient;
using System.Threading.Tasks;

namespace BitexTradingBot.Core.DataAccess.DataInvoke
{
    public interface IHttpClientApi
    {
        Task<TResponse> InvokeService<TResponse>(ApiClientOptions options) where TResponse : class;
    }
}
