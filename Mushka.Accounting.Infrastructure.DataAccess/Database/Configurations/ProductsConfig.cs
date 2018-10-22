using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal class ProductsConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder
                .Property(product => product.Id)
                .HasColumnName("Id");

            builder
                .HasKey(product => product.Id);

            builder
                .Property(product => product.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder
                .Property(product => product.Code)
                .HasColumnName("Code")
                .IsRequired();

            builder
                .Property(product => product.CreatedOn)
                .HasColumnName("CreatedOn")
                .IsRequired();
        }
    }
}