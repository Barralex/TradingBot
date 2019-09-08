using BitexTradingBot.Core.DataAccess.DataBase.Models;
using BitexTradingBot.Core.DataAccess.DataBase.Seeders;
using Microsoft.EntityFrameworkCore;

namespace BitexTradingBot.Core.DataAccess.DataBase.Contexts
{
    public class BitexTradingBotContext : DbContext
    {
        public BitexTradingBotContext(DbContextOptions<BitexTradingBotContext> options) : base(options)
        { }

        public DbSet<Trading> Trading { get; set; }
        public DbSet<TradingTransaction> TradingTransaction { get; set; }
        public DbSet<OrderType> OrderType { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderTypeSeed());
            modelBuilder.ApplyConfiguration(new OrderStatusSeed());
        }

    }
}
