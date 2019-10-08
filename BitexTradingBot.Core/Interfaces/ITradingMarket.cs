using System.Threading.Tasks;

namespace BitexTradingBot.Core.Interfaces
{
    public interface ITradingMarket
    {
        Task<TResponse> GetTickers<TResponse>() where TResponse : class;

        Task<TResponse> GetBtcPrice<TResponse>() where TResponse : class;
    }
}