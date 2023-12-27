using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting_Software.Migrations
{
    /// <inheritdoc />
    public partial class productwarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_WareHouses_Warehouseid",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Warehouseid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Warehouseid",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Warehouseid",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Warehouseid",
                table: "Products",
                column: "Warehouseid");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WareHouses_Warehouseid",
                table: "Products",
                column: "Warehouseid",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
