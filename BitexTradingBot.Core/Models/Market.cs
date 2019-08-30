using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BitexTradingBot.Core.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Market
    {
        [JsonProperty("data.id")]
        public string Id { get; set; }

        [JsonProperty("data.type")]
        public string Type { get; set; }

        [JsonProperty("data.relationships.bids.data")]
        public List<TradingPrices> Bids { get; set; } = new List<TradingPrices>();

        [JsonProperty("data.relationships.asks.data")]
        public List<TradingPrices> Aks { get; set; } = new List<TradingPrices>();
    }

    public class TradingPrices
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }
}


