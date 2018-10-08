using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal static class ProductConfiguration
    {
        public static void ConfigureProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");

            modelBuilder.Entity<Product>()
                .Property(product => product.Id)
                .HasColumnName("Id");

            modelBuilder.Entity<Product>()
                .HasKey(product => product.Id);

            modelBuilder.Entity<Product>()
                .Property(product => product.Name)
                .HasColumnName("Name")
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(product => product.Code)
                .HasColumnName("Code")
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(product => product.CreatedOn)
                .HasColumnName("CreatedOn")
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products);
        }
    }
}