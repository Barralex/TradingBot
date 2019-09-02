using Newtonsoft.Json;

namespace BitexTradingBot.Core.Models.Requests
{
    public class BidRequest
    {

        public BidRequest(double amount, double price, string orderbook, string orderType)
        {
            Data.Attributes.Amount = amount;
            Data.Attributes.Price = price;
            Data.Attributes.OrderbookCode = orderbook;
            Data.Type = orderType;
        }

        public BidRequestDetails Data { get; set; } = new BidRequestDetails();
    }

    public class BidRequestDetails
    {
        public string Type { get; set; }
        public BidRequestAttributes Attributes { get; set; } = new BidRequestAttributes();
    }

    public class BidRequestAttributes
    {
        public double Amount { get; set; }
        public double Price { get; set; }

        [JsonProperty("orderbook_code")]
        public string OrderbookCode { get; set; }
    }
}
