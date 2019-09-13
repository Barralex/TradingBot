using BitexTradingBot.Core.DataAccess.DataBase.Contexts;
using BitexTradingBot.Core.DataAccess.DataBase.Enums;
using BitexTradingBot.Core.DataAccess.DataBase.Models;
using BitexTradingBot.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitexTradingBot.BitexTradingBot.Impl
{
    public class StrategyDatabase : IStrategyDatabase
    {
        private readonly BitexTradingBotContext _context;

        public StrategyDatabase(BitexTradingBotContext context)
        {
            _context = context;
        }

        public async Task UpdateTradingOrder(TradingOrderDetails attemptOrderData, Trading actualOrder)
        {
            actualOrder.TradingTransactions.Add(CreateTradingTransactionObject(attemptOrderData));
            _context.Trading.Update(actualOrder);
            await _context.SaveChangesAsync();
        }

        public async Task CreateTradingOrder(TradingOrderDetails attemptOrderData)
        {
            var tradingObject = new Trading
            {
                StarDate = DateTime.Now,
                IsActive = true,
                InitialBalance = attemptOrderData.Attributes.Amount.Value,
                TradingTransactions = new List<TradingTransaction>
                    {
                         CreateTradingTransactionObject(attemptOrderData)
                    }
            };

            _context.Trading.Add(tradingObject);
            await _context.SaveChangesAsync();
        }

        public Trading GetLastTransaction()
        {
            return _context.Trading.Include(x => x.TradingTransactions).Where(s => s.IsActive).FirstOrDefault();
        }

        public async Task UpdateOrderStatus(Trading activeTrading, OrderStatusEnum statusChangeReason)
        {
            activeTrading.TradingTransactions.Last().OrderStatusId = (int)statusChangeReason;
            _context.Trading.Update(activeTrading);
            await _context.SaveChangesAsync();
        }

        private TradingTransaction CreateTradingTransactionObject(TradingOrderDetails attemptOrderData)
        {
            return new TradingTransaction
            {
                OrderTypeId = (int)OrderTypeEnum.Bid,
                Amount = attemptOrderData.Attributes.Amount.Value,
                CryptocurrencyPrice = attemptOrderData.Attributes.Price.Value,
                ExpectedOperationResult = (attemptOrderData.Attributes.Amount.Value * attemptOrderData.Attributes.Price.Value),
                StarDate = DateTime.Now,
                OrderStatusId = (int)OrderStatusEnum.Open,
                ExchangeOperationId = attemptOrderData.Id,
            };
        }
    }
}