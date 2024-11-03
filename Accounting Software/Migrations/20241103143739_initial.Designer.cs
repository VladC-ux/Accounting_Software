﻿// <auto-generated />
using System;
using Accounting_Software.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Accounting_Software.Migrations
{
    [DbContext(typeof(DBContextAccounting))]
    [Migration("20241103143739_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Accounting_Software.Data.Entites.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Mass")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SellerId")
                        .HasColumnType("int");

                    b.Property<int>("Unitofmass")
                        .HasColumnType("int");

                    b.Property<int?>("WareHouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.HasIndex("WareHouseId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Accounting_Software.Data.Entites.SoldList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Unitofmass")
                        .HasColumnType("int");

                    b.Property<int>("WareHouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WareHouseId");

                    b.ToTable("SoldLists");
                });

            modelBuilder.Entity("Accounting_Software.Data.Entites.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WareHouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WareHouseId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Accounting_Software.Date.Entites.Seller", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("Accounting_Software.Date.Entites.WareHouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Balance")
                        .HasColumnType("float");

                    b.Property<int?>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateBuy")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Mass")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("Productid")
                        .HasColumnType("int");

                    b.Property<int>("Unitofmass")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Productid");

                    b.ToTable("WareHouses");
                });

            modelBuilder.Entity("StoreProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Mass")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SellerId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<int>("Unitofmass")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreProducts");
                });

            modelBuilder.Entity("Accounting_Software.Data.Entites.Product", b =>
                {
                    b.HasOne("Accounting_Software.Date.Entites.Seller", "Seller")
                        .WithMany("Products")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Accounting_Software.Date.Entites.WareHouse", "WareHouse")
                        .WithMany()
                        .HasForeignKey("WareHouseId");

                    b.Navigation("Seller");

                    b.Navigation("WareHouse");
                });

            modelBuilder.Entity("Accounting_Software.Data.Entites.SoldList", b =>
                {
                    b.HasOne("Accounting_Software.Date.Entites.WareHouse", "WareHouse")
                        .WithMany()
                        .HasForeignKey("WareHouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WareHouse");
                });

            modelBuilder.Entity("Accounting_Software.Data.Entites.Store", b =>
                {
                    b.HasOne("Accounting_Software.Date.Entites.WareHouse", null)
                        .WithMany("Stores")
                        .HasForeignKey("WareHouseId");
                });

            modelBuilder.Entity("Accounting_Software.Date.Entites.WareHouse", b =>
                {
                    b.HasOne("Accounting_Software.Data.Entites.Product", "Product")
                        .WithMany()
                        .HasForeignKey("Productid");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("StoreProduct", b =>
                {
                    b.HasOne("Accounting_Software.Data.Entites.Product", null)
                        .WithMany("StoreProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Accounting_Software.Data.Entites.Store", "Store")
                        .WithMany("StoreProducts")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Accounting_Software.Data.Entites.Product", b =>
                {
                    b.Navigation("StoreProducts");
                });

            modelBuilder.Entity("Accounting_Software.Data.Entites.Store", b =>
                {
                    b.Navigation("StoreProducts");
                });

            modelBuilder.Entity("Accounting_Software.Date.Entites.Seller", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Accounting_Software.Date.Entites.WareHouse", b =>
                {
                    b.Navigation("Stores");
                });
#pragma warning restore 612, 618
        }
    }
}