using Microsoft.EntityFrameworkCore.Migrations;

namespace Bouquet.DataAccess.Migrations
{
    public partial class UpdateDeliverMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "ShoppingCarts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "ShoppingCarts");
        }
    }
}
