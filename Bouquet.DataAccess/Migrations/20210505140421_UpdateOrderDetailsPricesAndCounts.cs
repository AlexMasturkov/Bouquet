using Microsoft.EntityFrameworkCore.Migrations;

namespace Bouquet.DataAccess.Migrations
{
    public partial class UpdateOrderDetailsPricesAndCounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count2",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Count3",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price2",
                table: "OrderDetails",
                type: "decimal(7, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price3",
                table: "OrderDetails",
                type: "decimal(7, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count2",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Count3",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Price2",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Price3",
                table: "OrderDetails");
        }
    }
}
