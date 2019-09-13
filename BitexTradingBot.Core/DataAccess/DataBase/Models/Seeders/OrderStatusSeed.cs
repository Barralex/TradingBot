using BitexTradingBot.Core.DataAccess.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace BitexTradingBot.Core.DataAccess.DataBase.Seeders
{
    public class OrderStatusSeed : IEntityTypeConfiguration<OrderStatus>
    {

        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatus");

            builder.HasData
            (
                new OrderStatus
                {
                    Id = 1,
                    Description = "Open"
                },
                new OrderStatus
                {
                    Id = 2,
                    Description = "Finished"
                },
                new OrderStatus
                {
                    Id = 3,
                    Description = "Manually canceled"
                },
                new OrderStatus
                {
                    Id = 4,
                    Description = "Canceled by price change"
                }
            );
        }

    }

}
