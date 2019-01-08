using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class OrderProductConfig : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProducts");

            builder.HasKey(op => new { op.ProductId, op.OrderId });

            builder.HasOne(op => op.Product)
                .WithMany(product => product.Orders)
                .HasForeignKey(op => op.ProductId);

            builder.HasOne(op => op.Order)
                .WithMany(del => del.Products)
                .HasForeignKey(op => op.OrderId);
            
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