using Accounting_Software.Repositories;
using Accounting_Software.Service;
using Microsoft.AspNetCore.Authentication;
using Accounting_Software.Data;
using Microsoft.EntityFrameworkCore;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.UnitOfWorkk;




namespace Accounting_Software
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DBContextAccounting>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("AccountingDatabase")));

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IUserServiceInterface, UserService>();
            builder.Services.AddScoped<IUserRepositoryInterface, UserRepository>();

            builder.Services.AddScoped<IUnitofWork, UnitOfWork>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Index}/{id?}");

            app.Run();
        }
    }
}