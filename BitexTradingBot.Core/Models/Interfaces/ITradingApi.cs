using System.Threading.Tasks;

namespace BitexTradingBot.Core.Interfaces
{
    public interface ITradingApi
    {
        Task<TResponse> GetTickers<TResponse>() where TResponse : class;
        Task<TResponse> PlaceBidOrder<TResponse>(object request) where TResponse : class;
        Task<TResponse> CancelBidOrder<TResponse>(int bid) where TResponse : class;
    }
}
