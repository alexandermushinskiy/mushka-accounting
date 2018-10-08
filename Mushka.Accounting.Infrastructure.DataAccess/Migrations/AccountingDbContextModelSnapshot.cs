﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Infrastructure.DataAccess.Database;
using System;

namespace Mushka.Accounting.Infrastructure.DataAccess.Migrations
{
    [DbContext(typeof(AccountingDbContext))]
    partial class AccountingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<bool>("IsSizesRequired")
                        .HasColumnName("IsSizesRequired");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.Property<string>("Sizes")
                        .HasColumnName("Sizes");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.Delivery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("BatchNumber")
                        .IsRequired()
                        .HasColumnName("BatchNumber");

                    b.Property<decimal>("DeliveryCost")
                        .HasColumnName("DeliveryCost");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnName("DeliveryDate");

                    b.Property<bool>("IsDraft")
                        .HasColumnName("IsDraft");

                    b.Property<int>("PaymentMethod")
                        .HasColumnName("PaymentMethod");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnName("RequestDate");

                    b.Property<Guid?>("SupplierId");

                    b.Property<decimal>("TotalCost")
                        .HasColumnName("TotalCost");

                    b.Property<decimal>("TransferFee")
                        .HasColumnName("TransferFee");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.DeliveryProduct", b =>
                {
                    b.Property<Guid>("DeliveryId");

                    b.Property<Guid>("ProductId");

                    b.Property<int>("Amount")
                        .HasColumnName("Amount");

                    b.Property<int>("CostPerItem")
                        .HasColumnName("CostPerItem");

                    b.Property<string>("Notes")
                        .HasColumnName("Notes");

                    b.HasKey("DeliveryId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("DeliveryProducts");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.DeliveryService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cost");

                    b.Property<Guid?>("DeliveryId");

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryId");

                    b.ToTable("DeliveryService");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<Guid?>("CategoryId");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("CreatedOn");

                    b.Property<int>("DeliveriesNumber");

                    b.Property<int>("LastDeliveryCount");

                    b.Property<DateTime>("LastDeliveryDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.Property<int>("TotalCount");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.ProductSizeItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<int>("Amount")
                        .HasColumnName("Amount");

                    b.Property<Guid?>("DeliveryProductDeliveryId");

                    b.Property<Guid?>("DeliveryProductProductId");

                    b.Property<Guid?>("ProductId");

                    b.Property<string>("Size")
                        .HasColumnName("Size");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("DeliveryProductDeliveryId", "DeliveryProductProductId");

                    b.ToTable("SizeItems");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.Supplier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .HasColumnName("Address");

                    b.Property<string>("Comments")
                        .HasColumnName("Comments");

                    b.Property<string>("ContactPerson")
                        .HasColumnName("ContactPerson");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("CreatedOn");

                    b.Property<string>("Email")
                        .HasColumnName("Email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.Property<string>("PaymentConditions")
                        .HasColumnName("PaymentConditions");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnName("Phone");

                    b.Property<string>("Services")
                        .HasColumnName("Services");

                    b.Property<string>("WebSite")
                        .HasColumnName("WebSite");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.Delivery", b =>
                {
                    b.HasOne("Mushka.Accounting.Domain.Entities.Supplier", "Supplier")
                        .WithMany("Deliveries")
                        .HasForeignKey("SupplierId");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.DeliveryProduct", b =>
                {
                    b.HasOne("Mushka.Accounting.Domain.Entities.Delivery", "Delivery")
                        .WithMany("Products")
                        .HasForeignKey("DeliveryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Accounting.Domain.Entities.Product", "Product")
                        .WithMany("Deliveries")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.DeliveryService", b =>
                {
                    b.HasOne("Mushka.Accounting.Domain.Entities.Delivery", "Delivery")
                        .WithMany("Services")
                        .HasForeignKey("DeliveryId");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.Product", b =>
                {
                    b.HasOne("Mushka.Accounting.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("Mushka.Accounting.Domain.Entities.ProductSizeItem", b =>
                {
                    b.HasOne("Mushka.Accounting.Domain.Entities.Product", "Product")
                        .WithMany("SizeItems")
                        .HasForeignKey("ProductId");

                    b.HasOne("Mushka.Accounting.Domain.Entities.DeliveryProduct")
                        .WithMany("SizeItems")
                        .HasForeignKey("DeliveryProductDeliveryId", "DeliveryProductProductId");
                });
#pragma warning restore 612, 618
        }
    }
}
