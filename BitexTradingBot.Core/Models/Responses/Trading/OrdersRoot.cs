using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BitexTradingBot.Core.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class OrdersRoot
    {
        [JsonProperty("data")]
        public List<TradingOrderDetails> Orders { get; set; } = new List<TradingOrderDetails>();
    }
}