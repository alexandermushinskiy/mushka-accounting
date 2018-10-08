using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal static class SupplierConf
    {
        public static void ConfigureSupplier(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>().ToTable("Suppliers");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.Id)
                .HasColumnName("Id");

            modelBuilder.Entity<Supplier>()
                .HasKey(supplier => supplier.Id);

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.Name)
                .HasColumnName("Name")
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.Address)
                .HasColumnName("Address");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.Email)
                .HasColumnName("Email");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.Phone)
                .HasColumnName("Phone")
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.WebSite)
                .HasColumnName("WebSite");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.ContactPerson)
                .HasColumnName("ContactPerson");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.PaymentConditions)
                .HasColumnName("PaymentConditions");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.Services)
                .HasColumnName("Services");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.Comments)
                .HasColumnName("Comments");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.CreatedOn)
                .HasColumnName("CreatedOn");

            modelBuilder.Entity<Supplier>()
                .HasMany(supplier => supplier.Deliveries)
                .WithOne(delivery => delivery.Supplier);
        }
    }
}