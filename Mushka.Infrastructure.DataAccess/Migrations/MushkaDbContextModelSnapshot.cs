﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Migrations
{
    [DbContext(typeof(MushkaDbContext))]
    partial class MushkaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new { Id = new Guid("88cd0f34-9d4a-4e45-be97-8899a97fb82c"), Name = "Sock" },
                        new { Id = new Guid("0e7be1de-267c-4c0a-8ee9-aba0a267f27a"), Name = "Pack" },
                        new { Id = new Guid("b425d75b-2e72-45f0-a55d-3ba400051e5f"), Name = "Other" }
                    );
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Delivery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<decimal>("Cost")
                        .HasColumnName("Cost")
                        .HasColumnType("Money");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnName("DeliveryDate");

                    b.Property<int>("PaymentMethod")
                        .HasColumnName("PaymentMethod");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnName("RequestDate");

                    b.Property<decimal>("TransferFee")
                        .HasColumnName("TransferFee")
                        .HasColumnType("Money");

                    b.HasKey("Id");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.DeliveryProduct", b =>
                {
                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("DeliveryId");

                    b.Property<Guid>("SizeId");

                    b.Property<decimal>("PriceForItem")
                        .HasColumnName("PriceForItem")
                        .HasColumnType("Money");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.HasKey("ProductId", "DeliveryId", "SizeId");

                    b.HasIndex("DeliveryId");

                    b.HasIndex("SizeId");

                    b.ToTable("DeliveryProducts");
                });

            modelBuilder.Entity("Mushka.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnName("OrderDate");

                    b.HasKey("Id");

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

                    b.HasData(
                        new { Id = new Guid("eccef8a9-2c41-4270-9001-d0eb7e21b9e2"), Name = "36-39" },
                        new { Id = new Guid("fb8356a5-1629-4f9f-9b51-3d40e0e55f84"), Name = "39-42" },
                        new { Id = new Guid("2dfa21ef-5eed-462f-b5e5-06ee31281ba2"), Name = "41-45" },
                        new { Id = new Guid("6e519491-8fd8-45f2-992e-270b01f25971"), Name = "43-46" }
                    );
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

                    b.HasOne("Mushka.Domain.Entities.Size", "Size")
                        .WithMany("DeliveryProducts")
                        .HasForeignKey("SizeId")
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
