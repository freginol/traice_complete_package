using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagment.App.Data.Migrations
{
    public partial class AddInventoryItemQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "InventoryItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "InventoryItems");
        }
    }
}
