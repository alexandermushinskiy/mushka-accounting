using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database
{
    internal static class CategoryConfiguration
    {
        public static void ConfigureCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("category");

            modelBuilder.Entity<Category>()
                .Property(category => category.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Category>()
                .HasKey(category => category.Id);

            modelBuilder.Entity<Category>()
                .Property(category => category.Name)
                .HasColumnName("name")
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(category => category.IsSizesRequired)
                .HasColumnName("isSizesRequired")
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(category => category.Sizes)
                .HasColumnName("sizes");
        }
    }
}