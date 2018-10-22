using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal class CategoriesConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder
                .Property(category => category.Id)
                .HasColumnName("Id");

            builder
                .HasKey(category => category.Id);

            builder
                .Property(category => category.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder
                .HasMany(category => category.Products)
                .WithOne(product => product.Category);
        }
    }
}