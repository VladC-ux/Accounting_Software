using Accounting_Software.Repositories;
using Accounting_Software.Service;
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