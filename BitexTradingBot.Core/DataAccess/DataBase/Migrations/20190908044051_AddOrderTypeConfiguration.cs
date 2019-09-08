using Microsoft.EntityFrameworkCore.Migrations;

namespace BitexTradingBot.Core.Migrations
{
    public partial class AddOrderTypeConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderType",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "Bid" });

            migrationBuilder.InsertData(
                table: "OrderType",
                columns: new[] { "Id", "Description" },
                values: new object[] { 2, "Ask" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderType",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
