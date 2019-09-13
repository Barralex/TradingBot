using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BitexTradingBot.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trading",
                columns: table => new
                {
                    TradingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StarDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    InitialBalance = table.Column<double>(nullable: false),
                    FinalBalance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trading", x => x.TradingId);
                });

            migrationBuilder.CreateTable(
                name: "TradingTransaction",
                columns: table => new
                {
                    TradingTransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExchangeOperationId = table.Column<string>(nullable: true),
                    TradingId = table.Column<int>(nullable: false),
                    OrderTypeId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    CryptocurrencyPrice = table.Column<double>(nullable: false),
                    ExpectedOperationResult = table.Column<double>(nullable: false),
                    StarDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    OrderStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradingTransaction", x => x.TradingTransactionId);
                    table.ForeignKey(
                        name: "FK_TradingTransaction_OrderStatus_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradingTransaction_OrderType_OrderTypeId",
                        column: x => x.OrderTypeId,
                        principalTable: "OrderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradingTransaction_Trading_TradingId",
                        column: x => x.TradingId,
                        principalTable: "Trading",
                        principalColumn: "TradingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "Finished" },
                    { 3, "Manually canceled" },
                    { 4, "Canceled by price change" }
                });

            migrationBuilder.InsertData(
                table: "OrderType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Bid" },
                    { 2, "Ask" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradingTransaction_OrderStatusId",
                table: "TradingTransaction",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TradingTransaction_OrderTypeId",
                table: "TradingTransaction",
                column: "OrderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TradingTransaction_TradingId",
                table: "TradingTransaction",
                column: "TradingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradingTransaction");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "OrderType");

            migrationBuilder.DropTable(
                name: "Trading");
        }
    }
}
