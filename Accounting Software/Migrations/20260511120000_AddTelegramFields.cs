using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting_Software.Migrations
{
    /// <inheritdoc />
    public partial class AddTelegramFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TelegramChatId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramLinkCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramUsername",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TelegramNotifySales",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "TelegramNotifyLowStock",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "TelegramDailyReport",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "TelegramChatId", table: "Users");
            migrationBuilder.DropColumn(name: "TelegramLinkCode", table: "Users");
            migrationBuilder.DropColumn(name: "TelegramUsername", table: "Users");
            migrationBuilder.DropColumn(name: "TelegramNotifySales", table: "Users");
            migrationBuilder.DropColumn(name: "TelegramNotifyLowStock", table: "Users");
            migrationBuilder.DropColumn(name: "TelegramDailyReport", table: "Users");
        }
    }
}
