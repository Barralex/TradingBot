using BitexTradingBot.Core.Interfaces;
using System.Threading.Tasks;

namespace BitexTradingBot
{
    public class WebJobEntryPoint
    {
        private readonly IWebJobConfiguration _webJobConfiguration;
        public readonly IStrategy _strategy;

        public WebJobEntryPoint(IWebJobConfiguration webJobConfiguration, IStrategy strategy)
        {
            _webJobConfiguration = webJobConfiguration;
            _strategy = strategy; ;
        }

        public async Task Run()
        {
            await _strategy.Start();
        }
    }
}