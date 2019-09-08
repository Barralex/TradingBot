using System;
using System.Collections.Generic;

namespace BitexTradingBot.Core.DataAccess.DataBase.Models
{
    public class Trading
    {
        public int TradingId { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool OrderStatus { get; set; }
        public double InitialBalance { get; set; }
        public double FinalBalance { get; set; }

        public ICollection<TradingTransaction> TradingTransactions { get; set; }
    }
}
