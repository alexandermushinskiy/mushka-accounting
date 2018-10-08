using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal static class WarehouseConfiguration
    {
        public static void ConfigureWarehouse(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>().ToTable("Warehouses");

            modelBuilder.Entity<Warehouse>()
                .Property(wh => wh.Id)
                .HasColumnName("Id");

            modelBuilder.Entity<Warehouse>()
                .HasKey(wh => wh.Id);

            modelBuilder.Entity<Warehouse>()
                .Property(wh => wh.Size)
                .HasColumnName("Size")
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(wh => wh.Amount)
                .HasColumnName("Amount")
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .HasOne(wh => wh.Product);
        }
    }
}