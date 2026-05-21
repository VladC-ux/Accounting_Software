using Accounting_Software.Repositories;
using Accounting_Software.Service;
using Accounting_Software.Data;
using Microsoft.EntityFrameworkCore;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using QuestPDF.Infrastructure;
using System.Globalization;
using Telegram.Bot;

namespace Accounting_Software
{
    public class Program
    {
        public static void Main(string[] args)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var enUs = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = enUs;
            CultureInfo.DefaultThreadCurrentUICulture = enUs;

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DBContextAccounting>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("AccountingDatabase")));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Auth/Login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                });

            builder.Services.AddLocalization();
            builder.Services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
            builder.Services.AddScoped<ISellerService, SellerService>();
            builder.Services.AddScoped<ISellerRepository, SellerRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStoreService, StoreService>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<IStoreProductService, StoreProductService>();
            builder.Services.AddScoped<IStoreProductRepository, StoreProductRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
            builder.Services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();
            builder.Services.AddScoped<IUnitofWork, Accounting_Software.UnitOfWork.UnitOfWork>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<IInvoicePdfService, InvoicePdfService>();

            // ── Telegram bot ──────────────────────────────────────────────
            var botToken = builder.Configuration["Telegram:BotToken"];
            if (!string.IsNullOrWhiteSpace(botToken) && botToken != "YOUR_BOT_TOKEN_HERE")
            {
                builder.Services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken));
                builder.Services.AddHostedService<TelegramBotHostedService>();
            }
            builder.Services.AddSingleton<ITelegramNotifier, TelegramNotifier>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var supportedUICultures = new[]
            {
                enUs,
                new CultureInfo("hy-AM"),
                new CultureInfo("ru-RU"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUs),
                SupportedCultures = new[] { enUs },
                SupportedUICultures = supportedUICultures
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.Run();
        }
    }
}