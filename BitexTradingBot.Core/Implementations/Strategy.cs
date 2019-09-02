using System.Threading.Tasks;
using BitexTradingBot.Core.DataAccess.DataInvoke;
using BitexTradingBot.Core.Interfaces;

namespace BitexTradingBot.Core.Implementations
{
    public class SimpleStrategy : IStrategy
    {
        private readonly IHttpClientApi _httpClientApi;

        public SimpleStrategy(IHttpClientApi httpClientApi)
        {
            _httpClientApi = httpClientApi;
        }

        public Task BuyAtMarket()
        {
            throw new System.NotImplementedException();
        }

        public Task OnStarted()
        {
            throw new System.NotImplementedException();
        }

        public Task RegisterOrder()
        {
            throw new System.NotImplementedException();
        }

        public Task SellAtMarket()
        {
            throw new System.NotImplementedException();
        }
    }
}
