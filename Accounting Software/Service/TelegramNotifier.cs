using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Accounting_Software.Service
{
    public class TelegramNotifier : ITelegramNotifier
    {
        private readonly ITelegramBotClient? _bot;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TelegramNotifier> _logger;

        public TelegramNotifier(
            IServiceScopeFactory scopeFactory,
            ILogger<TelegramNotifier> logger,
            IServiceProvider sp)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _bot = sp.GetService(typeof(ITelegramBotClient)) as ITelegramBotClient;
        }

        public bool IsConfigured => _bot != null;

        public async Task SendMessageAsync(long chatId, string text)
        {
            if (_bot == null) return;
            try
            {
                await _bot.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Telegram send failed for chat {ChatId}", chatId);
            }
        }

        public async Task NotifySaleAsync(int userId, string productName, decimal price, string? storeName)
        {
            if (_bot == null) return;

            using var scope = _scopeFactory.CreateScope();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = userRepo.GetUserById(userId);
            if (user?.TelegramChatId == null || !user.TelegramNotifySales) return;

            var store = string.IsNullOrEmpty(storeName) ? "" : $"\n🏪 <b>Store:</b> {Escape(storeName)}";
            var text =
                "🟢 <b>New sale</b>\n" +
                $"📦 <b>Product:</b> {Escape(productName)}\n" +
                $"💵 <b>Amount:</b> {price:N2}{store}\n" +
                $"🕒 {DateTime.Now:dd.MM.yyyy HH:mm}";

            await SendMessageAsync(user.TelegramChatId.Value, text);
        }

        public async Task NotifyLowStockAsync(int userId, string productName, string storeName, int remaining)
        {
            if (_bot == null) return;

            using var scope = _scopeFactory.CreateScope();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = userRepo.GetUserById(userId);
            if (user?.TelegramChatId == null || !user.TelegramNotifyLowStock) return;

            var text =
                "⚠️ <b>Low stock!</b>\n" +
                $"📦 <b>Product:</b> {Escape(productName)}\n" +
                $"🏪 <b>Store:</b> {Escape(storeName)}\n" +
                $"📉 <b>Remaining:</b> {remaining} pcs\n" +
                "Time to restock.";

            await SendMessageAsync(user.TelegramChatId.Value, text);
        }

        public async Task SendDailyReportAsync(int userId)
        {
            if (_bot == null) return;

            using var scope = _scopeFactory.CreateScope();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var txService = scope.ServiceProvider.GetRequiredService<ITransactionHistoryService>();

            var user = userRepo.GetUserById(userId);
            if (user?.TelegramChatId == null) return;

            var txs = txService.GetHistoryByUserId(userId);
            var today = DateTime.Today;
            var todayTxs = txs.Where(t => t.SoldDate.Date == today).ToList();

            var sales = todayTxs.Where(t => t.typeofAction == "Sale").ToList();
            var purchases = todayTxs.Where(t => t.typeofAction == "Add").ToList();

            var revenue = sales.Sum(t => t.Total);
            var spent = purchases.Sum(t => t.Total);
            var profit = revenue - spent;
            var avgCheck = sales.Count > 0 ? revenue / sales.Count : 0;

            var top = sales
                .GroupBy(t => t.ProductName)
                .Select(g => new { Name = g.Key, Sum = g.Sum(t => t.Total), Cnt = g.Count() })
                .OrderByDescending(x => x.Sum)
                .Take(3)
                .ToList();

            var topBlock = top.Count == 0
                ? "—"
                : string.Join("\n", top.Select((p, i) => $"{i + 1}. {Escape(p.Name)} — {p.Sum:N2} ({p.Cnt} pcs)"));

            var text =
                $"📊 <b>Daily summary — {today:dd.MM.yyyy}</b>\n\n" +
                $"💰 <b>Revenue:</b> {revenue:N2}\n" +
                $"🛒 <b>Purchases:</b> {spent:N2}\n" +
                $"📈 <b>Profit:</b> {(profit >= 0 ? "+" : "")}{profit:N2}\n\n" +
                $"🧾 <b>Sales:</b> {sales.Count}\n" +
                $"💳 <b>Average check:</b> {avgCheck:N2}\n\n" +
                $"🏆 <b>Top products:</b>\n{topBlock}";

            await SendMessageAsync(user.TelegramChatId.Value, text);
        }

        private static string Escape(string? s) =>
            string.IsNullOrEmpty(s) ? "" : s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }
}
