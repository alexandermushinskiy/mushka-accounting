using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class ContactPersonConfig : IEntityTypeConfiguration<ContactPerson>
    {
        public void Configure(EntityTypeBuilder<ContactPerson> builder)
        {
            builder.ToTable("ContactPersons");

            builder
                .Property(cp => cp.Id)
                .HasColumnName("Id");

            builder.HasKey(cp => cp.Id);

            builder
                .Property(cp => cp.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(cp => cp.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(cp => cp.Position)
                .HasColumnName("Position")
                .HasMaxLength(255);

            builder
                .Property(cp => cp.City)
                .HasColumnName("City")
                .HasMaxLength(255);

            builder
                .Property(cp => cp.Email)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}