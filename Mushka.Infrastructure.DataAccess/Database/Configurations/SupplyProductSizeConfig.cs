using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class SupplyProductSizeConfig : IEntityTypeConfiguration<SupplyProductSize>
    {
        public void Configure(EntityTypeBuilder<SupplyProductSize> builder)
        {
            builder.ToTable("SupplyProductSizes");

            builder.HasKey(ps => new { ps.ProductId, ps.SupplyId, ps.SizeId });

            builder.HasOne(ps => ps.Size)
                .WithMany(size => size.SupplyProductSizes)
                .HasForeignKey(ps => ps.SizeId);

            builder.HasOne(ps => ps.Product)
                .WithMany(size => size.ProductSizes)
                .HasForeignKey(ps => new { ps.ProductId, ps.SupplyId });

            builder.Property(ps => ps.Quantity).HasColumnName("Quantity");
        }
    }
}