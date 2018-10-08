using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations
{
    internal static class CategoryConf
    {
        public static void ConfigureCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Categories");

            modelBuilder.Entity<Category>()
                .Property(category => category.Id)
                .HasColumnName("Id");

            modelBuilder.Entity<Category>()
                .HasKey(category => category.Id);

            modelBuilder.Entity<Category>()
                .Property(category => category.Name)
                .HasColumnName("Name")
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(category => category.IsSizesRequired)
                .HasColumnName("IsSizesRequired")
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(category => category.Sizes)
                .HasColumnName("Sizes");

            modelBuilder.Entity<Category>()
                .HasMany(category => category.Products)
                .WithOne(product => product.Category);
        }
    }
}