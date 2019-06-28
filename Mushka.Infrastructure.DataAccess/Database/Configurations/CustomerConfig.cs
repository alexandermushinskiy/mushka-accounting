using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder
                .Property(client => client.Id)
                .HasColumnName("Id");

            builder.HasKey(client => client.Id);

            builder
                .Property(client => client.FirstName)
                .IsRequired();

            builder
                .Property(client => client.LastName)
                .IsRequired();

            builder
                .Property(client => client.Email);

            builder
                .Property(client => client.Phone)
                .IsRequired();
        }
    }
}