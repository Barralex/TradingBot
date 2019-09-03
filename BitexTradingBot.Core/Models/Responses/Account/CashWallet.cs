using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;

namespace BitexTradingBot.Core.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class CashWallet
    {
        [JsonProperty("data.id")]
        public string Id { get; set; }
        [JsonProperty("data.type")]
        public string Type { get; set; }

        [JsonProperty("data.attributes")]
        public CashWalletAttributes Attributes { get; set; } = new CashWalletAttributes();
    }

    public class CashWalletAttributes
    {
        [JsonProperty("balance")]
        public string Balance { get; set; }
        [JsonProperty("available")]
        public string Available { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}


