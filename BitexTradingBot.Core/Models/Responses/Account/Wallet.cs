using BitexTradingBot.Core.Helpers;
using Newtonsoft.Json;

namespace BitexTradingBot.Core.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Wallet
    {
        [JsonProperty("data.id")]
        public string Id { get; set; }

        [JsonProperty("data.type")]
        public string Type { get; set; }

        [JsonProperty("data.attributes")]
        public WalletAttributes Attributes { get; set; } = new WalletAttributes();
    }

    public class WalletAttributes
    {
        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("available")]
        public double Available { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}