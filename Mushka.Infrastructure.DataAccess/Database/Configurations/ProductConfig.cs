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
                .Property(product => product.VendorCode)
                .HasColumnName("VendorCode")
                .IsRequired();

            builder
                .Property(product => product.RecommendedPrice)
                .HasColumnName("RecommendedPrice")
                .HasColumnType("Money");

            builder
                .Property(product => product.CreatedOn)
                .HasColumnName("CreatedOn")
                .IsRequired();

            builder
                .Property(ps => ps.Quantity)
                .HasColumnName("Quantity")
                .HasDefaultValue(0);

            builder
                .Property(ps => ps.IsArchival)
                .HasColumnName("IsArchival");

            builder
                .HasOne(product => product.Size)
                .WithMany(size => size.Products)
                .HasForeignKey(product => product.SizeId);

            builder
                .HasOne(prod => prod.Category)
                .WithMany(cat => cat.Products)
                .HasForeignKey(prod => prod.CategoryId);

            builder.HasIndex(product => product.VendorCode).IsUnique();
            builder.HasIndex(product => new { product.Name, product.SizeId }).IsUnique();
        }
    }
}