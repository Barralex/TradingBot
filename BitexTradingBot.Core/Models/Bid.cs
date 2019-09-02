using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;

namespace BitexTradingBot.Core.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Bid
    {
        [JsonProperty("data.id")]
        public string Id { get; set; }

        [JsonProperty("data.type")]
        public string Type { get; set; }

        [JsonProperty("data.attributes")]
        public BidAttributes Attributes { get; set; } = new BidAttributes();
    }

    public class BidAttributes
    {
        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("remaining_amount")]
        public double? RemainingAmount { get; set; }

        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}


