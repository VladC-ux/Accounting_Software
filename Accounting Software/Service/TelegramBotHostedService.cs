using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Accounting_Software.Service
{
    public class TelegramBotHostedService : BackgroundService
    {
        private readonly ITelegramBotClient _bot;
        private readonly IServiceProvider _services;
        private readonly ILogger<TelegramBotHostedService> _logger;
        private readonly IConfiguration _config;

        public TelegramBotHostedService(
            ITelegramBotClient bot,
            IServiceProvider services,
            ILogger<TelegramBotHostedService> logger,
            IConfiguration config)
        {
            _bot = bot;
            _services = services;
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message }
            };

            try
            {
                var me = await _bot.GetMeAsync(stoppingToken);
                _logger.LogInformation("Telegram bot started: @{Username}", me.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Telegram bot failed to start. Check BotToken in appsettings.");
                return;
            }

            _ = Task.Run(() => RunDailyReportLoopAsync(stoppingToken), stoppingToken);

            await _bot.ReceiveAsync(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
        {
            if (update.Message is not { Text: { } text } msg) return;
            var chatId = msg.Chat.Id;
            var command = text.Trim().Split(' ', 2);
            var cmd = command[0].ToLowerInvariant();
            var arg = command.Length > 1 ? command[1].Trim() : "";

            using var scope = _services.CreateScope();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var notifier = scope.ServiceProvider.GetRequiredService<ITelegramNotifier>();
            var txService = scope.ServiceProvider.GetRequiredService<ITransactionHistoryService>();
            var storeProductService = scope.ServiceProvider.GetRequiredService<IStoreProductService>();

            try
            {
                switch (cmd)
                {
                    case "/start":
                        await bot.SendTextMessageAsync(chatId,
                            "👋 <b>Hi! This is the Accounting Software bot.</b>\n\n" +
                            "To link your account, open the <b>Telegram</b> section in the app, " +
                            "copy the code and send it to me:\n" +
                            "<code>/link ABCD1234</code>\n\n" +
                            "Once linked, you'll receive sale notifications " +
                            "and can request reports right here.\n\n" +
                            "Commands: /help",
                            parseMode: ParseMode.Html, cancellationToken: ct);
                        break;

                    case "/help":
                        await bot.SendTextMessageAsync(chatId,
                            "<b>Available commands:</b>\n\n" +
                            "/link CODE — link your account\n" +
                            "/today — today's report\n" +
                            "/week — last 7 days\n" +
                            "/top — top 5 products (last 30 days)\n" +
                            "/low — low-stock products\n" +
                            "/unlink — unlink account",
                            parseMode: ParseMode.Html, cancellationToken: ct);
                        break;

                    case "/link":
                        await HandleLinkAsync(bot, userRepo, chatId, msg.From, arg, ct);
                        break;

                    case "/unlink":
                        await HandleUnlinkAsync(bot, userRepo, chatId, ct);
                        break;

                    case "/today":
                        if (RequireLinked(userRepo, chatId, out var u1))
                            await notifier.SendDailyReportAsync(u1!.Id);
                        else
                            await bot.SendTextMessageAsync(chatId, "Link your account first: /link CODE", cancellationToken: ct);
                        break;

                    case "/week":
                        if (RequireLinked(userRepo, chatId, out var u2))
                            await SendWeekReportAsync(bot, txService, u2!.Id, chatId, ct);
                        else
                            await bot.SendTextMessageAsync(chatId, "Link your account first: /link CODE", cancellationToken: ct);
                        break;

                    case "/top":
                        if (RequireLinked(userRepo, chatId, out var u3))
                            await SendTopAsync(bot, txService, u3!.Id, chatId, ct);
                        else
                            await bot.SendTextMessageAsync(chatId, "Link your account first: /link CODE", cancellationToken: ct);
                        break;

                    case "/low":
                        if (RequireLinked(userRepo, chatId, out _))
                            await SendLowStockAsync(bot, storeProductService, chatId, ct);
                        else
                            await bot.SendTextMessageAsync(chatId, "Link your account first: /link CODE", cancellationToken: ct);
                        break;

                    default:
                        await bot.SendTextMessageAsync(chatId,
                            "Unknown command. Send /help to see the list.",
                            cancellationToken: ct);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling Telegram command {Cmd}", cmd);
                try
                {
                    await bot.SendTextMessageAsync(chatId, "Something went wrong. Try again later.", cancellationToken: ct);
                }
                catch { }
            }
        }

        private static bool RequireLinked(IUserRepository repo, long chatId, out Data.Entities.User? user)
        {
            user = repo.GetByTelegramChatId(chatId);
            return user != null;
        }

        private async Task HandleLinkAsync(ITelegramBotClient bot, IUserRepository repo, long chatId, Telegram.Bot.Types.User? from, string code, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                await bot.SendTextMessageAsync(chatId,
                    "Usage: <code>/link ABCD1234</code>\nGet your code in the app under <b>Telegram</b>.",
                    parseMode: ParseMode.Html, cancellationToken: ct);
                return;
            }

            var user = repo.GetByTelegramLinkCode(code.Trim());
            if (user == null)
            {
                await bot.SendTextMessageAsync(chatId, "❌ Invalid code. Generate a new one in the app.", cancellationToken: ct);
                return;
            }

            user.TelegramChatId = chatId;
            user.TelegramUsername = from?.Username;
            user.TelegramLinkCode = null;
            repo.Update(user);

            await bot.SendTextMessageAsync(chatId,
                $"✅ <b>Account linked!</b>\nHi, {user.Name}.\n\n" +
                "You'll now receive sale notifications and can request reports.\n\n" +
                "Try: /today",
                parseMode: ParseMode.Html, cancellationToken: ct);
        }

        private async Task HandleUnlinkAsync(ITelegramBotClient bot, IUserRepository repo, long chatId, CancellationToken ct)
        {
            var user = repo.GetByTelegramChatId(chatId);
            if (user == null)
            {
                await bot.SendTextMessageAsync(chatId, "This chat is not linked.", cancellationToken: ct);
                return;
            }
            user.TelegramChatId = null;
            user.TelegramUsername = null;
            repo.Update(user);
            await bot.SendTextMessageAsync(chatId, "🔌 Account unlinked.", cancellationToken: ct);
        }

        private async Task SendWeekReportAsync(ITelegramBotClient bot, ITransactionHistoryService txService, int userId, long chatId, CancellationToken ct)
        {
            var txs = txService.GetHistoryByUserId(userId);
            var from = DateTime.Today.AddDays(-6);
            var period = txs.Where(t => t.SoldDate.Date >= from).ToList();

            var sales = period.Where(t => t.typeofAction == "Sale").ToList();
            var purchases = period.Where(t => t.typeofAction == "Add").ToList();
            var revenue = sales.Sum(t => t.Total);
            var spent = purchases.Sum(t => t.Total);
            var profit = revenue - spent;

            var text =
                $"📅 <b>7-day report</b> ({from:dd.MM} – {DateTime.Today:dd.MM})\n\n" +
                $"💰 Revenue: <b>{revenue:N2}</b>\n" +
                $"🛒 Purchases: <b>{spent:N2}</b>\n" +
                $"📈 Profit: <b>{(profit >= 0 ? "+" : "")}{profit:N2}</b>\n" +
                $"🧾 Sales: <b>{sales.Count}</b>";

            await bot.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html, cancellationToken: ct);
        }

        private async Task SendTopAsync(ITelegramBotClient bot, ITransactionHistoryService txService, int userId, long chatId, CancellationToken ct)
        {
            var txs = txService.GetHistoryByUserId(userId);
            var from = DateTime.Today.AddDays(-29);
            var top = txs
                .Where(t => t.typeofAction == "Sale" && t.SoldDate.Date >= from)
                .GroupBy(t => t.ProductName)
                .Select(g => new { Name = g.Key, Sum = g.Sum(t => t.Total), Cnt = g.Count() })
                .OrderByDescending(x => x.Sum)
                .Take(5)
                .ToList();

            if (top.Count == 0)
            {
                await bot.SendTextMessageAsync(chatId, "No sales in the last 30 days.", cancellationToken: ct);
                return;
            }

            var body = string.Join("\n", top.Select((p, i) => $"{i + 1}. <b>{Escape(p.Name)}</b> — {p.Sum:N2} ({p.Cnt} pcs)"));
            await bot.SendTextMessageAsync(chatId, $"🏆 <b>Top 5 products (last 30 days)</b>\n\n{body}", parseMode: ParseMode.Html, cancellationToken: ct);
        }

        private async Task SendLowStockAsync(ITelegramBotClient bot, IStoreProductService storeProductService, long chatId, CancellationToken ct)
        {
            const int threshold = 5;
            var items = storeProductService.GetAll()
                .Where(sp => sp.Count > 0 && sp.Count <= threshold)
                .OrderBy(sp => sp.Count)
                .Take(15)
                .ToList();

            if (items.Count == 0)
            {
                await bot.SendTextMessageAsync(chatId, $"✅ No products with stock ≤ {threshold}.", cancellationToken: ct);
                return;
            }

            var body = string.Join("\n",
                items.Select(i => $"⚠️ <b>{Escape(i.ProductName)}</b> — {i.Count} pcs ({Escape(i.StoreName)})"));
            await bot.SendTextMessageAsync(chatId, $"📉 <b>Low stock (≤ {threshold} pcs)</b>\n\n{body}",
                parseMode: ParseMode.Html, cancellationToken: ct);
        }

        private async Task RunDailyReportLoopAsync(CancellationToken ct)
        {
            var hour = int.TryParse(_config["Telegram:DailyReportHour"], out var h) ? h : 21;

            while (!ct.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var next = now.Date.AddHours(hour);
                if (next <= now) next = next.AddDays(1);
                var delay = next - now;

                try { await Task.Delay(delay, ct); }
                catch (TaskCanceledException) { return; }

                using var scope = _services.CreateScope();
                var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var notifier = scope.ServiceProvider.GetRequiredService<ITelegramNotifier>();

                foreach (var user in userRepo.GetAll().Where(u => u.TelegramChatId != null && u.TelegramDailyReport))
                {
                    try { await notifier.SendDailyReportAsync(user.Id); }
                    catch (Exception ex) { _logger.LogWarning(ex, "Daily report failed for user {Id}", user.Id); }
                }
            }
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken ct)
        {
            var msg = ex switch
            {
                ApiRequestException api => $"Telegram API error [{api.ErrorCode}]: {api.Message}",
                _ => ex.ToString()
            };
            _logger.LogError("Telegram polling error: {Msg}", msg);
            return Task.CompletedTask;
        }

        private static string Escape(string? s) =>
            string.IsNullOrEmpty(s) ? "" : s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }
}
