using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class PaymentCardConfig : IEntityTypeConfiguration<PaymentCard>
    {
        public void Configure(EntityTypeBuilder<PaymentCard> builder)
        {
            builder.ToTable("PaymentCards");

            builder
                .Property(cn => cn.Id)
                .HasColumnName("Id");

            builder.HasKey(cn => cn.Id);

            builder
                .Property(cn => cn.Number)
                .HasColumnName("Number")
                .HasMaxLength(16)
                .IsRequired();
            
            builder
                .Property(cn => cn.Owner)
                .HasColumnName("Owner")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(cn => cn.Number).IsUnique();
        }
    }
}