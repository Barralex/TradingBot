using System.Threading.Tasks;

namespace BitexTradingBot.Core.Interfaces
{
    public interface ITradingApi
    {
        Task<TResponse> GetTickers<TResponse>() where TResponse : class;
    }
}
