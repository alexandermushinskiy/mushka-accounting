using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class ExhibitionProductConfig : IEntityTypeConfiguration<ExhibitionProduct>
    {
        public void Configure(EntityTypeBuilder<ExhibitionProduct> builder)
        {
            builder.ToTable("ExhibitionProducts");

            builder.HasKey(ep => new { ep.ProductId, ep.ExhibitionId });

            builder.HasOne(ep => ep.Product)
                .WithMany(product => product.Exhibitions)
                .HasForeignKey(op => op.ProductId);

            builder.HasOne(ep => ep.Exhibition)
                .WithMany(del => del.Products)
                .HasForeignKey(op => op.ExhibitionId);

            builder.Property(op => op.Quantity)
                .HasColumnName("Quantity");

            builder.Property(op => op.UnitPrice)
                .HasColumnName("UnitPrice")
                .HasColumnType("Money")
                .IsRequired();

            builder.Property(dp => dp.CostPrice)
                .HasColumnName("CostPrice")
                .HasColumnType("Money")
                .IsRequired();
        }
    }
}