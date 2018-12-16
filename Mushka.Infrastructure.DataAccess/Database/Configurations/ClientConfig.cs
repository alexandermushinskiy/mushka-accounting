using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder
                .Property(client => client.Id)
                .HasColumnName("Id");

            builder.HasKey(client => client.Id);

            builder.Property(client => client.FirstName).IsRequired();
            builder.Property(client => client.LastName).IsRequired();
            builder.Property(client => client.MiddleName);
            builder.Property(client => client.Email);
            builder.Property(client => client.Phone);
        }
    }
}