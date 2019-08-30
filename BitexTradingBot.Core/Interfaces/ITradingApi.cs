using System.Threading.Tasks;

namespace BitexTradingBot.Core.Interfaces
{
    public interface ITradingApi
    {
        Task<TResponse> GetMarket<TResponse>() where TResponse : class;
    }
}
