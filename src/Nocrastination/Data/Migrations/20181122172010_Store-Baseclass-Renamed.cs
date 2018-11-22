using Microsoft.EntityFrameworkCore.Migrations;

namespace Nocrastination.Data.Migrations
{
    public partial class StoreBaseclassRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Stores",
                table: "Stores");

            migrationBuilder.RenameTable(
                name: "Stores",
                newName: "StoreItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreItems",
                table: "StoreItems",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreItems",
                table: "StoreItems");

            migrationBuilder.RenameTable(
                name: "StoreItems",
                newName: "Stores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stores",
                table: "Stores",
                column: "Id");
        }
    }
}
