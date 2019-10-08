using BitexTradingBot.Core.DataAccess.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitexTradingBot.Core.DataAccess.DataBase.Seeders
{
    public class OrderTypeSeed : IEntityTypeConfiguration<OrderType>
    {
        public void Configure(EntityTypeBuilder<OrderType> builder)
        {
            builder.ToTable("OrderType");

            builder.HasData
            (
                new OrderType
                {
                    Id = 1,
                    Description = "Bid"
                },
                new OrderType
                {
                    Id = 2,
                    Description = "Ask"
                }
            );
        }
    }
}