namespace Accounting_Software.Service_Interfaces
{
    public interface ITelegramNotifier
    {
        bool IsConfigured { get; }
        Task NotifySaleAsync(int userId, string productName, decimal price, string? storeName);
        Task NotifyLowStockAsync(int userId, string productName, string storeName, int remaining);
        Task SendDailyReportAsync(int userId);
        Task SendMessageAsync(long chatId, string text);
    }
}
