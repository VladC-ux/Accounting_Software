namespace Accounting_Software.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Balance { get; set; } = 0m;
        public List<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();

        // Auth fields
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        // Telegram bot integration
        public long? TelegramChatId { get; set; }
        public string? TelegramLinkCode { get; set; }
        public string? TelegramUsername { get; set; }
        public bool TelegramNotifySales { get; set; } = true;
        public bool TelegramNotifyLowStock { get; set; } = true;
        public bool TelegramDailyReport { get; set; } = true;
    }
}
