using System;

namespace BitexTradingBot.Core.DataAccess.DataBase.Models
{
    public class TradingTransaction
    {
        public int TradingTransactionId { get; set; }
        public string ExchangeOperationId { get; set; }

        public int TradingId { get; set; }
        public Trading Trading { get; set; }

        public int OrderTypeId { get; set; }
        public OrderType OrderType { get; set; }

        public double Amount { get; set; }
        public double CryptocurrencyPrice { get; set; }
        public double ExpectedOperationResult { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }

        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}