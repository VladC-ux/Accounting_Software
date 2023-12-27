using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting_Software.Migrations
{
    /// <inheritdoc />
    public partial class initialware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_WareHouses_WareHouseId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "WareHouseId",
                table: "Products",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_WareHouseId",
                table: "Products",
                newName: "IX_Products_WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WareHouses_WarehouseId",
                table: "Products",
                column: "WarehouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_WareHouses_WarehouseId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "Products",
                newName: "WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_WarehouseId",
                table: "Products",
                newName: "IX_Products_WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WareHouses_WareHouseId",
                table: "Products",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
