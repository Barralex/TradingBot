using BitexTradingBot.Core.Interfaces;

namespace BitexTradingBot.Core.Models
{
    public class WebJobConfiguration : IWebJobConfiguration
    {
        public string BitexApiUrl { get; set; }
        public string BitexDefaultMarket { get; set; }
        public string BitexApiKey { get; set; }
    }
}
