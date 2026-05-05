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
    }
}
