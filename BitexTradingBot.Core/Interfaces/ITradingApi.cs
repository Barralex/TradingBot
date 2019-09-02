using System.Threading.Tasks;

namespace BitexTradingBot.Core.Interfaces
{
    public interface ITradingApi : ITradingMarket
    {
        Task<TResponse> GetOwnOrders<TResponse>() where TResponse : class;
        Task<TResponse> PlaceOrder<TResponse>(object request, string orderType) where TResponse : class;
        Task CancelOrder(string orderId, string orderType);
    }
}
