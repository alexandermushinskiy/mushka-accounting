using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class SupplyConfig : IEntityTypeConfiguration<Supply>
    {
        public void Configure(EntityTypeBuilder<Supply> builder)
        {
            builder.ToTable("Supplies");

            builder
                .Property(del => del.Id)
                .HasColumnName("Id");

            builder.HasKey(del => del.Id);

            builder
                .Property(del => del.RequestDate)
                .HasColumnName("RequestDate")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property(del => del.ReceivedDate)
                .HasColumnName("ReceivedDate")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property(del => del.Cost)
                .HasColumnName("Cost")
                .HasColumnType("Money")
                .IsRequired();
            
            builder
                .Property(del => del.CostMethod)
                .HasColumnName("CostMethod")
                .IsRequired();

            builder
                .Property(del => del.Prepayment)
                .HasColumnName("Prepayment")
                .HasColumnType("Money");

            builder
                .Property(del => del.PrepaymentMethod)
                .HasColumnName("PrepaymentMethod");

            builder
                .Property(del => del.DeliveryCost)
                .HasColumnName("DeliveryCost")
                .HasColumnType("Money");

            builder
                .Property(del => del.DeliveryCostMethod)
                .HasColumnName("DeliveryCostMethod");

            builder
                .Property(del => del.BankFee)
                .HasColumnName("BankFee")
                .HasColumnType("Money");
            
            builder
                .Property(del => del.TotalCost)
                .HasColumnName("TotalCost")
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(del => del.Notes)
                .HasColumnName("Notes");

            builder
                .HasOne(del => del.Supplier)
                .WithMany(sup => sup.Supplies)
                .HasForeignKey(del => del.SupplierId);
        }
    }
}