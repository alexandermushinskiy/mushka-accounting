using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal class ProductSizeConfig : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            builder.ToTable("ProductSizes");

            builder.HasKey(ps => new { ps.ProductId, ps.SizeId });

            builder.HasOne(ps => ps.Product)
                .WithMany(product => product.Sizes)
                .HasForeignKey(ps => ps.ProductId);

            builder.HasOne(ps => ps.Size)
                .WithMany(product => product.Products)
                .HasForeignKey(ps => ps.SizeId);
        }
    }
}