using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal static class DeliveryProductConf
    {
        public static void ConfigureDeliveryProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryProduct>().ToTable("DeliveryProducts");

            modelBuilder.Entity<DeliveryProduct>()
                .HasKey(dp => new { dp.DeliveryId, dp.ProductId });

            modelBuilder.Entity<DeliveryProduct>()
                .HasOne(bc => bc.Delivery)
                .WithMany(b => b.Products)
                .HasForeignKey(bc => bc.DeliveryId);

            modelBuilder.Entity<DeliveryProduct>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.Deliveries)
                .HasForeignKey(bc => bc.ProductId);

            modelBuilder.Entity<DeliveryProduct>()
                .Property(delivery => delivery.Amount)
                .HasColumnName("Amount")
                .IsRequired();

            modelBuilder.Entity<DeliveryProduct>()
                .Property(delivery => delivery.CostPerItem)
                .HasColumnName("CostPerItem")
                .IsRequired();

            modelBuilder.Entity<DeliveryProduct>()
                .Property(delivery => delivery.Notes)
                .HasColumnName("Notes");

            modelBuilder.Entity<DeliveryProduct>()
                .HasMany(delivery => delivery.SizeItems);
        }
    }
}