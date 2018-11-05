using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class DeliveryConfig : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("Deliveries");

            builder
                .Property(del => del.Id)
                .HasColumnName("Id");

            builder.HasKey(del => del.Id);

            builder
                .Property(del => del.Cost)
                .HasColumnName("Cost")
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(del => del.DeliveryDate)
                .HasColumnName("DeliveryDate")
                .IsRequired();

            builder
                .Property(del => del.PaymentMethod)
                .HasColumnName("PaymentMethod")
                .IsRequired();

            builder
                .Property(del => del.RequestDate)
                .HasColumnName("RequestDate")
                .IsRequired();

            builder
                .Property(del => del.TransferFee)
                .HasColumnName("TransferFee")
                .HasColumnType("Money");
        }
    }
}