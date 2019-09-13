﻿// <auto-generated />
using System;
using BitexTradingBot.Core.DataAccess.DataBase.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BitexTradingBot.Core.Migrations
{
    [DbContext(typeof(BitexTradingBotContext))]
    partial class BitexTradingBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BitexTradingBot.Core.DataAccess.DataBase.Models.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("OrderStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Open"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Finished"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Manually canceled"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Canceled by price change"
                        });
                });

            modelBuilder.Entity("BitexTradingBot.Core.DataAccess.DataBase.Models.OrderType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("OrderType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Bid"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Ask"
                        });
                });

            modelBuilder.Entity("BitexTradingBot.Core.DataAccess.DataBase.Models.Trading", b =>
                {
                    b.Property<int>("TradingId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate");

                    b.Property<double>("FinalBalance");

                    b.Property<double>("InitialBalance");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("StarDate");

                    b.HasKey("TradingId");

                    b.ToTable("Trading");
                });

            modelBuilder.Entity("BitexTradingBot.Core.DataAccess.DataBase.Models.TradingTransaction", b =>
                {
                    b.Property<int>("TradingTransactionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<double>("CryptocurrencyPrice");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("ExchangeOperationId");

                    b.Property<double>("ExpectedOperationResult");

                    b.Property<int>("OrderStatusId");

                    b.Property<int>("OrderTypeId");

                    b.Property<DateTime>("StarDate");

                    b.Property<int>("TradingId");

                    b.HasKey("TradingTransactionId");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("OrderTypeId");

                    b.HasIndex("TradingId");

                    b.ToTable("TradingTransaction");
                });

            modelBuilder.Entity("BitexTradingBot.Core.DataAccess.DataBase.Models.TradingTransaction", b =>
                {
                    b.HasOne("BitexTradingBot.Core.DataAccess.DataBase.Models.OrderStatus", "OrderStatus")
                        .WithMany()
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BitexTradingBot.Core.DataAccess.DataBase.Models.OrderType", "OrderType")
                        .WithMany()
                        .HasForeignKey("OrderTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BitexTradingBot.Core.DataAccess.DataBase.Models.Trading", "Trading")
                        .WithMany("TradingTransactions")
                        .HasForeignKey("TradingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
