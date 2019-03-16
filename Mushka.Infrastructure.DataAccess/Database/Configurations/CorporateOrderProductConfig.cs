using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class CorporateOrderProductConfig : IEntityTypeConfiguration<CorporateOrderProduct>
    {
        public void Configure(EntityTypeBuilder<CorporateOrderProduct> builder)
        {
            builder.ToTable("CorporateOrderProducts");

            builder
                .Property(order => order.Id)
                .HasColumnName("Id");

            builder.HasKey(order => order.Id);

            builder.HasOne(op => op.CorporateOrder)
                .WithMany(del => del.Products)
                .HasForeignKey(op => op.CorporateOrderId);

            builder.Property(op => op.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(op => op.Quantity)
                .HasColumnName("Quantity")
                .IsRequired();

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