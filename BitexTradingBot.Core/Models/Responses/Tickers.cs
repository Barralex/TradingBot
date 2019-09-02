using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BitexTradingBot.Core.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Tickers
    {
        [JsonProperty("data")]
        public List<TickersDetails> TickersDetails { get; set; } = new List<TickersDetails>();
    }

    public class TickersDetails
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public TickersAttributes Attributes { get; set; } = new TickersAttributes();
    }

    public class TickersAttributes
    {
        [JsonProperty("last")]
        public double? Last { get; set; }

        [JsonProperty("open")]
        public double? Open { get; set; }

        [JsonProperty("high")]
        public double? High { get; set; }

        [JsonProperty("low")]
        public double? Low { get; set; }

        [JsonProperty("vwap")]
        public double? Vwap { get; set; }

        [JsonProperty("volume")]
        public double? Volume { get; set; }

        [JsonProperty("bid")]
        public double? Bid { get; set; }

        [JsonProperty("ask")]
        public double? Ask { get; set; }

        [JsonProperty("price_before_last")]
        public double? PriceBeforeLast { get; set; }
    }

}