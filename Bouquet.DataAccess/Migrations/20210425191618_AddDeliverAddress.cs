using Microsoft.EntityFrameworkCore.Migrations;

namespace Bouquet.DataAccess.Migrations
{
    public partial class AddDeliverAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ShoppingCarts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryName",
                table: "ShoppingCarts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ShoppingCarts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "ShoppingCarts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "ShoppingCarts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "ShoppingCarts",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "DeliveryName",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "ShoppingCarts");
        }
    }
}
