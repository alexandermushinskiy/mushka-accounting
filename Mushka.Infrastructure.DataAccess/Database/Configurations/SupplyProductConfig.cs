using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class SupplyProductConfig : IEntityTypeConfiguration<SupplyProduct>
    {
        public void Configure(EntityTypeBuilder<SupplyProduct> builder)
        {
            builder.ToTable("SupplyProducts");

            builder.HasKey(dp => new { dp.ProductId, dp.SupplyId });

            builder.HasOne(dp => dp.Product)
                .WithMany(product => product.Supplies)
                .HasForeignKey(dp => dp.ProductId);

            builder.HasOne(dp => dp.Supply)
                .WithMany(del => del.Products)
                .HasForeignKey(dp => dp.SupplyId);

            builder.Property(dp => dp.CostForItem)
                .HasColumnName("CostForItem")
                .HasColumnType("Money")
                .IsRequired();

            builder.Property(ps => ps.Quantity)
                .HasColumnName("Quantity")
                .IsRequired();
        }
    }
}