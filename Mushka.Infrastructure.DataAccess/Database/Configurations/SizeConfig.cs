using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class SizeConfig : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.ToTable("Sizes");

            builder
                .Property(size => size.Id)
                .HasColumnName("Id");

            builder
                .Property(product => product.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder.HasIndex(product => product.Name).IsUnique();
        }
    }
}