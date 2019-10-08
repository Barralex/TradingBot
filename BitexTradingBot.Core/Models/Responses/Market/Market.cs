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
        public List<Data> Bids { get; set; } = new List<Data>();

        [JsonProperty("data.relationships.asks.data")]
        public List<Data> Aks { get; set; } = new List<Data>();
    }
}