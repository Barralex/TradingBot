using System.Threading.Tasks;

namespace BitexTradingBot.Core.Interfaces
{
    public interface ITradingApi : ITradingMarket
    {
        Task<TResponse> GetOwnOrders<TResponse>() where TResponse : class;
        Task<TResponse> PlaceOrder<TResponse>(object request, string orderType) where TResponse : class;
        Task CancelOrder(string orderId, string orderType);
        Task<TResponse> GetCashWallet<TResponse>(string currency) where TResponse : class;
        Task<TResponse> GetOrder<TResponse>(string orderType, string orderId) where TResponse : class;
        Task<TResponse> GetCoinWallet<TResponse>(int walletId) where TResponse : class;
    }
}
