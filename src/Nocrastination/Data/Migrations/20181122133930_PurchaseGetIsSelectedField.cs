using Microsoft.EntityFrameworkCore.Migrations;

namespace Nocrastination.Data.Migrations
{
    public partial class PurchaseGetIsSelectedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Purchases",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Purchases");
        }
    }
}
