using Microsoft.EntityFrameworkCore.Migrations;

namespace BitexTradingBot.Core.Migrations
{
    public partial class AddOrderStatusConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "Finished" });

            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Description" },
                values: new object[] { 2, "Manually canceled" });

            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Description" },
                values: new object[] { 3, "Canceled by price change" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
