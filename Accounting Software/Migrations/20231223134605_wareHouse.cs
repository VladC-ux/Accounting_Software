using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting_Software.Migrations
{
    /// <inheritdoc />
    public partial class wareHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_WareHouseId",
                table: "Products",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WareHouses_WareHouseId",
                table: "Products",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_WareHouses_WareHouseId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WareHouseId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "Products");
        }
    }
}
