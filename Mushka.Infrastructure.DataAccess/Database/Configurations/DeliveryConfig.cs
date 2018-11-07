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
                .Property(del => del.RequestDate)
                .HasColumnName("RequestDate")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property(del => del.DeliveryDate)
                .HasColumnName("DeliveryDate")
                .HasColumnType("Date")
                .IsRequired();
            
            builder
                .Property(del => del.TransferFee)
                .HasColumnName("TransferFee")
                .HasColumnType("Money");

            builder
                .Property(del => del.BankFee)
                .HasColumnName("BankFee")
                .HasColumnType("Money");

            //builder
            //    .HasOne(del => del.Supplier)
            //    .WithMany(sup => sup.Deliveries)
            //    .HasForeignKey(del => del.SupplierId);
        }
    }
}