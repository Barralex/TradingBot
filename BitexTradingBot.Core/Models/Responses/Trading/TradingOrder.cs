using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;
using System;

namespace BitexTradingBot.Core.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class TradingOrder
    {
        [JsonProperty("data")]
        public TradingOrderDetails Details { get; set; } = new TradingOrderDetails();
    }

    public class TradingOrderDetails : Data
    {
        [JsonProperty("attributes")]
        public TradingOrderAttributes Attributes { get; set; } = new TradingOrderAttributes();
    }

    public class TradingOrderAttributes
    {
        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("remaining_amount")]
        public double? RemainingAmount { get; set; }

        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}