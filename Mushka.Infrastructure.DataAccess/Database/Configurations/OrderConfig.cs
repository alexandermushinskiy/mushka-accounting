using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder
                .Property(order => order.Id)
                .HasColumnName("Id");

            builder.HasKey(order => order.Id);

            builder
                .Property(order => order.Number)
                .HasColumnName("Number")
                .IsRequired();

            builder
                .Property(order => order.OrderDate)
                .HasColumnName("OrderDate")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property(order => order.Cost)
                .HasColumnName("Cost")
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(order => order.CostMethod)
                .HasColumnName("CostMethod")
                .IsRequired();

            builder
                .Property(order => order.Discount)
                .HasColumnName("Discount");

            builder
                .Property(order => order.Profit)
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(order => order.IsWholesale)
                .IsRequired();

            builder
                .Property(order => order.Notes)
                .HasColumnName("Notes");

            builder
                .Property(order => order.City)
                .IsRequired();

            builder
                .Property(order => order.Region)
                .IsRequired();

            builder.HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.CustomerId);

            builder.HasIndex(order => order.Number).IsUnique();
        }
    }
}