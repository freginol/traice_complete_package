using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagment.App.Data.Migrations
{
    public partial class AddInventoryActiveStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSoldOut",
                table: "InventoryItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSoldOut",
                table: "InventoryItems");
        }
    }
}
