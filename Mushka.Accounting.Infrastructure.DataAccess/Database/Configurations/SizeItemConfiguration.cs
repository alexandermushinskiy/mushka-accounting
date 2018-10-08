using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal static class SizeItemConfiguration
    {
        public static void ConfigureSizeItem(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductSizeItem>().ToTable("SizeItems");

            modelBuilder.Entity<ProductSizeItem>()
                .Property(si => si.Id)
                .HasColumnName("Id");

            modelBuilder.Entity<ProductSizeItem>()
                .HasKey(si => si.Id);

            modelBuilder.Entity<ProductSizeItem>()
                .Property(si => si.Size)
                .HasColumnName("Size");

            modelBuilder.Entity<ProductSizeItem>()
                .Property(si => si.Amount)
                .HasColumnName("Amount");

            modelBuilder.Entity<ProductSizeItem>()
                .HasOne(si => si.Product)
                .WithMany(product => product.SizeItems);
            
        }
    }
}
