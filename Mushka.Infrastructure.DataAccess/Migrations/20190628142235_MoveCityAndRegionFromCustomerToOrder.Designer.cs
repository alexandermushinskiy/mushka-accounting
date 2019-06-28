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
    [Migration("20190628142235_MoveCityAndRegionFromCustomerToOrder")]
    partial class MoveCityAndRegionFromCustomerToOrder
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

                    b.Property<bool>("IsAdditional")
                        .HasColumnName("IsAdditional");

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

            modelBuilder.Entity("Mushka.Domain.Entities.CorporateOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnName("City")
                        .HasMaxLength(255);

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnName("CompanyName")
                        .HasMaxLength(255);

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasColumnName("ContactPerson")
                        .HasMaxLength(255);

                    b.Property<decimal>("Cost")
                        .HasColumnName("Cost")
                        .HasColumnType("Money");

                    b.Property<int>("CostMethod")
                        .HasColumnName("CostMethod");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("CreatedOn")
                        .HasColumnType("Date");

                    b.Property<decimal?>("DeliveryCost")
                        .HasColumnName("DeliveryCost")
                        .HasColumnType("Money");

                    b.Property<int?>("DeliveryCostMethod")
                        .HasColumnName("DeliveryCostMethod");

                    b.Property<string>("Email")
                        .HasColumnName("Email")
                        .HasMaxLength(255);

                    b.Property<string>("Notes")
                        .HasColumnName("Notes");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnName("Number")
                        .HasMaxLength(255);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnName("Phone")
                        .HasMaxLength(255);

                    b.Property<decimal?>("Prepayment")
                        .HasColumnName("Prepayment")
                        .HasColumnType("Money");

                    b.Property<int?>("PrepaymentMethod")
                        .HasColumnName("PrepaymentMethod");

                    b.Property<decimal>("Profit")
                        .HasColumnName("Profit")
                        .HasColumnType("Money");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnName("Region")
                        .HasMaxLength(255);

                    b.Property<int?>("Tax")
                        .HasColumnName("Tax");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("CorporateOrders");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.CorporateOrderProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<Guid>("CorporateOrderId");

                    b.Property<decimal>("CostPrice")
                        .HasColumnName("CostPrice")
                        .HasColumnType("Money");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnName("UnitPrice")
                        .HasColumnType("Money");

                    b.HasKey("Id");

                    b.HasIndex("CorporateOrderId");

                    b.ToTable("CorporateOrderProducts");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Exhibition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<decimal?>("AccommodationCost")
                        .HasColumnName("AccommodationCost")
                        .HasColumnType("Money");

                    b.Property<int?>("AccommodationCostMethod")
                        .HasColumnName("AccommodationCostMethod");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnName("City")
                        .HasMaxLength(255);

                    b.Property<decimal?>("FareCost")
                        .HasColumnName("FareCost")
                        .HasColumnType("Money");

                    b.Property<int?>("FareCostMethod")
                        .HasColumnName("FareCostMethod");

                    b.Property<DateTime>("FromDate")
                        .HasColumnName("FromDate")
                        .HasColumnType("Date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(255);

                    b.Property<string>("Notes")
                        .HasColumnName("Notes");

                    b.Property<decimal>("ParticipationCost")
                        .HasColumnName("ParticipationCost")
                        .HasColumnType("Money");

                    b.Property<int>("ParticipationCostMethod")
                        .HasColumnName("ParticipationCostMethod");

                    b.Property<decimal>("Profit")
                        .HasColumnName("Profit")
                        .HasColumnType("Money");

                    b.Property<DateTime>("ToDate")
                        .HasColumnName("ToDate")
                        .HasColumnType("Date");

                    b.HasKey("Id");

                    b.ToTable("Exhibitions");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.ExhibitionProduct", b =>
                {
                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("ExhibitionId");

                    b.Property<decimal>("CostPrice")
                        .HasColumnName("CostPrice")
                        .HasColumnType("Money");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnName("UnitPrice")
                        .HasColumnType("Money");

                    b.HasKey("ProductId", "ExhibitionId");

                    b.HasIndex("ExhibitionId");

                    b.ToTable("ExhibitionProducts");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<int>("Category")
                        .HasColumnName("Category");

                    b.Property<decimal>("Cost")
                        .HasColumnName("Cost")
                        .HasColumnType("Money");

                    b.Property<int>("CostMethod")
                        .HasColumnName("CostMethod");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("CreatedOn")
                        .HasColumnType("Date");

                    b.Property<string>("Notes")
                        .HasColumnName("Notes");

                    b.Property<string>("Purpose")
                        .IsRequired()
                        .HasColumnName("Purpose")
                        .HasMaxLength(255);

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnName("SupplierName")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<decimal>("Cost")
                        .HasColumnName("Cost")
                        .HasColumnType("Money");

                    b.Property<int>("CostMethod")
                        .HasColumnName("CostMethod");

                    b.Property<Guid>("CustomerId");

                    b.Property<int?>("Discount")
                        .HasColumnName("Discount");

                    b.Property<bool>("IsWholesale");

                    b.Property<string>("Notes")
                        .HasColumnName("Notes");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnName("Number");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnName("OrderDate")
                        .HasColumnType("Date");

                    b.Property<decimal>("Profit")
                        .HasColumnType("Money");

                    b.Property<string>("Region")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.OrderProduct", b =>
                {
                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("OrderId");

                    b.Property<decimal>("CostPrice")
                        .HasColumnName("CostPrice")
                        .HasColumnType("Money");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnName("UnitPrice")
                        .HasColumnType("Money");

                    b.HasKey("ProductId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.PaymentCard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnName("Number")
                        .HasMaxLength(16);

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnName("Owner")
                        .HasMaxLength(255);

                    b.Property<Guid>("SupplierId");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("SupplierId");

                    b.ToTable("PaymentCards");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<Guid>("CategoryId");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("CreatedOn");

                    b.Property<bool>("IsArchival")
                        .HasColumnName("IsArchival");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Quantity")
                        .HasDefaultValue(0);

                    b.Property<decimal?>("RecommendedPrice")
                        .HasColumnName("RecommendedPrice")
                        .HasColumnType("Money");

                    b.Property<Guid?>("SizeId");

                    b.Property<string>("VendorCode")
                        .IsRequired()
                        .HasColumnName("VendorCode");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SizeId");

                    b.HasIndex("VendorCode")
                        .IsUnique();

                    b.HasIndex("Name", "SizeId")
                        .IsUnique()
                        .HasFilter("[SizeId] IS NOT NULL");

                    b.ToTable("Products");
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
                        .HasColumnName("Address");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
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

            modelBuilder.Entity("Mushka.Domain.Entities.Supply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<decimal?>("BankFee")
                        .HasColumnName("BankFee")
                        .HasColumnType("Money");

                    b.Property<decimal>("Cost")
                        .HasColumnName("Cost")
                        .HasColumnType("Money");

                    b.Property<int>("CostMethod")
                        .HasColumnName("CostMethod");

                    b.Property<decimal?>("DeliveryCost")
                        .HasColumnName("DeliveryCost")
                        .HasColumnType("Money");

                    b.Property<int?>("DeliveryCostMethod")
                        .HasColumnName("DeliveryCostMethod");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Notes")
                        .HasColumnName("Notes");

                    b.Property<decimal?>("Prepayment")
                        .HasColumnName("Prepayment")
                        .HasColumnType("Money");

                    b.Property<int?>("PrepaymentMethod")
                        .HasColumnName("PrepaymentMethod");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnName("ReceivedDate")
                        .HasColumnType("Date");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnName("RequestDate")
                        .HasColumnType("Date");

                    b.Property<Guid>("SupplierId");

                    b.Property<decimal>("TotalCost")
                        .HasColumnName("TotalCost")
                        .HasColumnType("Money");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("Supplies");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.SupplyProduct", b =>
                {
                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("SupplyId");

                    b.Property<decimal>("CostPrice")
                        .HasColumnName("CostPrice")
                        .HasColumnType("Money");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnName("UnitPrice")
                        .HasColumnType("Money");

                    b.HasKey("ProductId", "SupplyId");

                    b.HasIndex("SupplyId");

                    b.ToTable("SupplyProducts");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.ContactPerson", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Supplier", "Supplier")
                        .WithMany("ContactPersons")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.CorporateOrderProduct", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.CorporateOrder", "CorporateOrder")
                        .WithMany("Products")
                        .HasForeignKey("CorporateOrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.ExhibitionProduct", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Exhibition", "Exhibition")
                        .WithMany("Products")
                        .HasForeignKey("ExhibitionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Domain.Entities.Product", "Product")
                        .WithMany("Exhibitions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Order", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
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
                });

            modelBuilder.Entity("Mushka.Domain.Entities.PaymentCard", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Supplier", "Supplier")
                        .WithMany("PaymentCards")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Product", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Domain.Entities.Size", "Size")
                        .WithMany("Products")
                        .HasForeignKey("SizeId");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Supply", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Supplier", "Supplier")
                        .WithMany("Supplies")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mushka.Domain.Entities.SupplyProduct", b =>
                {
                    b.HasOne("Mushka.Domain.Entities.Product", "Product")
                        .WithMany("Supplies")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mushka.Domain.Entities.Supply", "Supply")
                        .WithMany("Products")
                        .HasForeignKey("SupplyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
