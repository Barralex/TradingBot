using System.Threading.Tasks;
using BitexTradingBot.Core.Interfaces;

namespace BitexTradingBot.Core.Implementations
{
    public class Strategy : IStrategy
    {
        private readonly ITradingApi _tradingApi;

        public Strategy(ITradingApi tradingApi)
        {
            _tradingApi = tradingApi;
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
