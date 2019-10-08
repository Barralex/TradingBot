using Newtonsoft.Json;

namespace BitexTradingBot.Core.Models.Requests
{
    public class TradingOrdenRequest
    {
        public TradingOrdenRequest(double amount, double price, string orderbook, string orderType)
        {
            Data.Attributes.Amount = amount;
            Data.Attributes.Price = price;
            Data.Attributes.OrderbookCode = orderbook;
            Data.Type = orderType;
        }

        public TradingOrdentDetails Data { get; set; } = new TradingOrdentDetails();
    }

    public class TradingOrdentDetails
    {
        public string Type { get; set; }
        public TradingOrdentAttributes Attributes { get; set; } = new TradingOrdentAttributes();
    }

    public class TradingOrdentAttributes
    {
        public double Amount { get; set; }
        public double Price { get; set; }

        [JsonProperty("orderbook_code")]
        public string OrderbookCode { get; set; }
    }
}