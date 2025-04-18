﻿using Accounting_Software.Date.Entites;
using Microsoft.EntityFrameworkCore;
using Accounting_Software.Data.Entites;

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

    }
}
