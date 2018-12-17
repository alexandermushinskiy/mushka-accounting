using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class DeliveryProductSizeConfig : IEntityTypeConfiguration<DeliveryProductSize>
    {
        public void Configure(EntityTypeBuilder<DeliveryProductSize> builder)
        {
            builder.ToTable("DeliveryProductSizes");

            builder.HasKey(ps => new { ps.ProductId, ps.DeliveryId, ps.SizeId });

            builder.HasOne(ps => ps.Size)
                .WithMany(size => size.DeliveryProductSizes)
                .HasForeignKey(ps => ps.SizeId);

            builder.HasOne(ps => ps.Product)
                .WithMany(size => size.ProductSizes)
                .HasForeignKey(ps => new { ps.ProductId, ps.DeliveryId });

            builder.Property(ps => ps.Quantity).HasColumnName("Quantity");
        }
    }
}