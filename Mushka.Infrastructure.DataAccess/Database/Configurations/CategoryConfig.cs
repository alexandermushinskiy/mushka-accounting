using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class CategoryConfig : IEntityTypeConfiguration<Category>
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
        }
    }
}