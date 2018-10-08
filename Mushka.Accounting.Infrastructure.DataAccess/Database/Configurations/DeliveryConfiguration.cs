using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal static class DeliveryConfiguration
    {
        public static void ConfigureDelivery(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Delivery>().ToTable("Deliveries");

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.Id)
                .HasColumnName("Id");

            modelBuilder.Entity<Delivery>()
                .HasKey(delivery => delivery.Id);

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.BatchNumber)
                .HasColumnName("BatchNumber")
                .IsRequired();

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.RequestDate)
                .HasColumnName("RequestDate")
                .IsRequired();

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.DeliveryDate)
                .HasColumnName("DeliveryDate")
                .IsRequired();

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.TransferFee)
                .HasColumnName("TransferFee");

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.DeliveryCost)
                .HasColumnName("DeliveryCost");

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.TotalCost)
                .HasColumnName("TotalCost")
                .IsRequired();

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.IsDraft)
                .HasColumnName("IsDraft");

            modelBuilder.Entity<Delivery>()
                .Property(delivery => delivery.PaymentMethod)
                .HasColumnName("PaymentMethod");

            modelBuilder.Entity<Delivery>()
                .HasOne(delivery => delivery.Supplier)
                .WithMany(supplier => supplier.Deliveries);

            modelBuilder.Entity<Delivery>()
                .HasMany(delivery => delivery.Services)
                .WithOne(service => service.Delivery);
        }
    }
}