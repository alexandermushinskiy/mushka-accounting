using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class ProductConfig : IEntityTypeConfiguration<Product>
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

            builder
                .HasOne(prod => prod.Category)
                .WithMany(cat => cat.Products)
                .HasForeignKey(prod => prod.CategoryId);
        }
    }
}