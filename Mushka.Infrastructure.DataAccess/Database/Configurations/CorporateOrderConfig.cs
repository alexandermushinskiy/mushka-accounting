using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class CorporateOrderConfig : IEntityTypeConfiguration<CorporateOrder>
    {
        public void Configure(EntityTypeBuilder<CorporateOrder> builder)
        {
            builder.ToTable("CorporateOrders");

            builder
                .Property(order => order.Id)
                .HasColumnName("Id");

            builder.HasKey(order => order.Id);

            builder
                .Property(order => order.Number)
                .HasColumnName("Number")
                .HasMaxLength(255)
                .IsRequired();
            
            builder.HasIndex(order => order.Number).IsUnique();

            builder
                .Property(order => order.CreatedOn)
                .HasColumnName("CreatedOn")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property(order => order.Cost)
                .HasColumnName("Cost")
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(order => order.CostMethod)
                .HasColumnName("CostMethod")
                .IsRequired();

            builder
                .Property(del => del.Prepayment)
                .HasColumnName("Prepayment")
                .HasColumnType("Money");

            builder
                .Property(del => del.PrepaymentMethod)
                .HasColumnName("PrepaymentMethod");

            builder
                .Property(del => del.DeliveryCost)
                .HasColumnName("DeliveryCost")
                .HasColumnType("Money");

            builder
                .Property(del => del.DeliveryCostMethod)
                .HasColumnName("DeliveryCostMethod");

            builder
                .Property(del => del.Tax)
                .HasColumnName("Tax");

            builder
                .Property(order => order.Profit)
                .HasColumnName("Profit")
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(client => client.CompanyName)
                .HasColumnName("CompanyName")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(client => client.ContactPerson)
                .HasColumnName("ContactPerson")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(client => client.Email)
                .HasColumnName("Email")
                .HasMaxLength(255);

            builder
                .Property(client => client.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(order => order.City)
                .HasColumnName("City")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(order => order.Region)
                .HasColumnName("Region")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(order => order.Notes)
                .HasColumnName("Notes");
        }
    }
}