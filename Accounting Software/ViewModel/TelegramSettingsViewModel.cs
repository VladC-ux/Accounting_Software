namespace Accounting_Software.ViewModel
{
    public class TelegramSettingsViewModel
    {
        public bool IsLinked { get; set; }
        public string? TelegramUsername { get; set; }
        public string? LinkCode { get; set; }
        public string? BotUsername { get; set; }

        public bool NotifySales { get; set; }
        public bool NotifyLowStock { get; set; }
        public bool DailyReport { get; set; }

        public bool BotConfigured { get; set; }
    }
}
