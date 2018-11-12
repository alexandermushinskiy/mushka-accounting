using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    //internal class PaymentConfig : IEntityTypeConfiguration<Payment>
    //{
    //    public void Configure(EntityTypeBuilder<Payment> builder)
    //    {
    //        builder.ToTable("Payments");

    //        builder
    //            .Property(p => p.Id)
    //            .HasColumnName("Id");

    //        builder.HasKey(p => p.Id);

    //        builder
    //            .Property(p => p.PaymentMethod)
    //            .HasColumnName("PaymentMethod")
    //            .IsRequired();

    //        builder
    //            .Property(p => p.CreditCardNumber)
    //            .HasColumnName("CreditCardNumber");

    //        builder
    //            .Property(p => p.Notes)
    //            .HasColumnName("Notes");
    //    }
    //}
}