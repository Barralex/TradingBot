using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;
using System;

namespace BitexTradingBot.Core.Models.Responses.Market
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Cryptocurrency
    {
        [JsonProperty("data.1.quote.USD")]
        public CryptocurrencyDetails Details { get; set; } = new CryptocurrencyDetails();
    }

    public class CryptocurrencyDetails

    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}