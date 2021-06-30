using Microsoft.EntityFrameworkCore.Migrations;

namespace Bouquet.DataAccess.Migrations
{
    public partial class UpdateProductWithOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LuxuryOption",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PremiumOption",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegularOption",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LuxuryOption",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PremiumOption",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RegularOption",
                table: "Products");
        }
    }
}
