using System.Threading.Tasks;
using BitexTradingBot.Core.Interfaces;

namespace BitexTradingBot.Core.Implementations
{
    public class BitexTradingApi : ITradingApi
    {
        public Task<TResponse> GetMarket<TResponse>() where TResponse : class
        {
            throw new System.NotImplementedException();
        }
    }
}
