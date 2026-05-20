using Accounting_Software.Enums;

namespace Accounting_Software.Service_Interfaces
{
    public interface ITelegramNotifier
    {
        bool IsConfigured { get; }
        Task NotifySaleAsync(int userId, string productName, decimal unitPrice, int count, ushort mass, Unit_of_mass unitOfMass, decimal total, string? storeName);
        Task NotifyLowStockAsync(int userId, string productName, string storeName, int remaining);
        Task SendDailyReportAsync(int userId);
        Task SendMessageAsync(long chatId, string text);
    }
}
