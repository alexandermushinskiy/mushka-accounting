using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class DeliveryProductConfig : IEntityTypeConfiguration<DeliveryProduct>
    {
        public void Configure(EntityTypeBuilder<DeliveryProduct> builder)
        {
            builder.ToTable("DeliveryProducts");

            builder.HasKey(dp => new { dp.ProductId, dp.DeliveryId, dp.SizeId });

            builder.HasOne(dp => dp.Product)
                .WithMany(product => product.Deliveries)
                .HasForeignKey(dp => dp.ProductId);

            builder.HasOne(dp => dp.Delivery)
                .WithMany(del => del.Products)
                .HasForeignKey(dp => dp.DeliveryId);

            builder.HasOne(dp => dp.Size)
                .WithMany(size => size.DeliveryProducts)
                .HasForeignKey(dp => dp.SizeId);

            builder.Property(dp => dp.Quantity)
                .HasColumnName("Quantity");

            builder.Property(dp => dp.PriceForItem)
                .HasColumnName("PriceForItem")
                .HasColumnType("Money");
        }
    }
}