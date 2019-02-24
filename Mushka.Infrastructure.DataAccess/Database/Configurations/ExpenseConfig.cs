using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class ExpenseConfig : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expenses");

            builder
                .Property(expense => expense.Id)
                .HasColumnName("Id");

            builder.HasKey(expense => expense.Id);

            builder
                .Property(expense => expense.CreatedOn)
                .HasColumnName("CreatedOn")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property(expense => expense.Cost)
                .HasColumnName("Cost")
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(expense => expense.CostMethod)
                .HasColumnName("CostMethod")
                .IsRequired();

            builder
                .Property(expense => expense.Category)
                .HasColumnName("Category")
                .IsRequired();

            builder
                .Property(expense => expense.Purpose)
                .HasColumnName("Purpose")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(expense => expense.SupplierName)
                .HasColumnName("SupplierName")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(expense => expense.Notes)
                .HasColumnName("Notes");
        }
    }
}