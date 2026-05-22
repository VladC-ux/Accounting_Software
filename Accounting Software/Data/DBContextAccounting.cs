using Accounting_Software.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accounting_Software.Data
{
    public class DBContextAccounting:DbContext
    {
        public DBContextAccounting(DbContextOptions<DBContextAccounting> options): base(options)
        {

        }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<TaxRule> TaxRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>(e =>
            {
                e.HasIndex(i => i.Number).IsUnique();
                e.Property(i => i.Number).HasMaxLength(32).IsRequired();
                e.Property(i => i.TaxRate).HasColumnType("decimal(5,2)");
                e.HasMany(i => i.Items)
                    .WithOne(it => it.Invoice!)
                    .HasForeignKey(it => it.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(i => i.Store)
                    .WithMany()
                    .HasForeignKey(i => i.StoreId)
                    .OnDelete(DeleteBehavior.SetNull);
                e.HasOne(i => i.User)
                    .WithMany()
                    .HasForeignKey(i => i.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InvoiceItem>(e =>
            {
                e.Property(it => it.Price).HasColumnType("decimal(18,2)");
                e.Property(it => it.ProductName).HasMaxLength(256).IsRequired();
            });

            modelBuilder.Entity<TaxRule>(e =>
            {
                e.Property(t => t.Name).HasMaxLength(128).IsRequired();
                e.Property(t => t.Rate).HasColumnType("decimal(5,2)");
                e.HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
