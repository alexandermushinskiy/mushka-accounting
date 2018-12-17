﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Migrations
{
    [DbContext(typeof(MushkaDbContext))]
    [Migration("20181216215634_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mushka.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<bool>("IsSizeRequired");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.Property<int>("Order")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Order")
                        .HasDefaultValue(100);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("MiddleName");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.ContactPerson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Email")
                        .HasColumnName("Email")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(255);

                    b.Property<string>("Phones")
                        .HasColumnName("Phones")
                        .HasMaxLength(255);

                    b.Property<Guid>("SupplierId");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("ContactPersons");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Delivery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<decimal>("BankFee")
                        .HasColumnName("BankFee")
                        .HasColumnType("Money");

                    b.Property<decimal>("Cost")
                        .HasColumnName("Cost")
                        .HasColumnType("Money");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnName("DeliveryDate")
                        .HasColumnType("Date");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnName("RequestDate")
                        .HasColumnType("Date");

                    b.Property<Guid?>("SupplierId");

                    b.Property<decimal>("TransferFee")
                        .HasColumnName("TransferFee")
                        .HasColumnType("Money");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.DeliveryProduct", b =>
                {
                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("DeliveryId");

                    b.Property<decimal>("PriceForItem")
                        .HasColumnName("PriceForItem")
                        .HasColumnType("Money");

                    b.HasKey("ProductId", "DeliveryId");

                    b.HasIndex("DeliveryId");

                    b.ToTable("DeliveryProducts");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.DeliveryProductSize", b =>
                {
                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("DeliveryId");

                    b.Property<Guid>("SizeId");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.HasKey("ProductId", "DeliveryId", "SizeId");

                    b.HasIndex("SizeId");

                    b.ToTable("DeliveryProductSizes");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<Guid>("ClientId");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnName("OrderDate")
                        .HasColumnType("Date");

                    b.Property<int>("PaymentType");

                    b.Property<string>("Region")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.OrderProduct", b =>
                {
                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("OrderId");

                    b.Property<Guid>("SizeId");

                    b.Property<decimal>("PriceForItem")
                        .HasColumnName("PriceForItem")
                        .HasColumnType("Money");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.HasKey("ProductId", "OrderId", "SizeId");

                    b.HasIndex("OrderId");

                    b.HasIndex("SizeId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<Guid>("CategoryId");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("CreatedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.ProductSize", b =>
                {
                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("SizeId");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.HasKey("ProductId", "SizeId");

                    b.HasIndex("SizeId");

                    b.ToTable("ProductSizes");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Size", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Supplier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnName("Address");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("Email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(255);

                    b.Property<string>("Notes")
                        .HasColumnName("Notes");

                    b.Property<string>("Service")
                        .HasColumnName("Service");

                    b.Property<string>("WebSite")
                        .HasColumnName("WebSite");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.ContactPerson", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Supplier", "Supplier")
                        .WithMany("ContactPersons")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Delivery", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Supplier")
                        .WithMany("Deliveries")
                        .HasForeignKey("SupplierId");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.DeliveryProduct", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Delivery", "Delivery")
                        .WithMany("Products")
                        .HasForeignKey("DeliveryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Domain.Entities.Product", "Product")
                        .WithMany("Deliveries")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.DeliveryProductSize", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Size", "Size")
                        .WithMany("DeliveryProductSizes")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Domain.Entities.DeliveryProduct", "Product")
                        .WithMany("ProductSizes")
                        .HasForeignKey("ProductId", "DeliveryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Order", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.OrderProduct", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Domain.Entities.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Domain.Entities.Size", "Size")
                        .WithMany("OrderProducts")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Product", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.ProductSize", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Product", "Product")
                        .WithMany("Sizes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Domain.Entities.Size", "Size")
                        .WithMany("Products")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}