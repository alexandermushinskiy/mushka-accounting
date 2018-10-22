using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal class SizesConfig : IEntityTypeConfiguration<Size>
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