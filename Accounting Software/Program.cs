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
            builder.Services.AddScoped<ISellerServiceInterface, SellerService>();
            builder.Services.AddScoped<ISellerRepositoryInterface, SellerRepository>();
            builder.Services.AddScoped<IProductServiceInterface, ProductService>();
            builder.Services.AddScoped<IProductRepositoryInterface, ProductRepository>();
            builder.Services.AddScoped<IWareHouseServiceInterface, WareHouseService>();
            builder.Services.AddScoped<IWareHouseRepositoryInterface, WareHouseRepository>();
            
            

            builder.Services.AddScoped<IUnitofWork, UnitOfWork>();


            var app = builder.Build();

           
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Seller}/{action=Index}/{id?}");

            app.Run();
        }
    }
}