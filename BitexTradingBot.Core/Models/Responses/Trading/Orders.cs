using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BitexTradingBot.Core.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Orders
    {
        [JsonProperty("data")]
        public List<TradingOrderDetails> Details { get; set; } = new List<TradingOrderDetails>();
    }

}


