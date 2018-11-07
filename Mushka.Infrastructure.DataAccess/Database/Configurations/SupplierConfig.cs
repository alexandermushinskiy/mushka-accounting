﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class SupplierConfig : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder
                .Property(sup => sup.Id)
                .HasColumnName("Id");

            builder.HasKey(sup => sup.Id);

            builder
                .Property(sup => sup.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(sup => sup.Name).IsUnique();

            builder
                .Property(sup => sup.Address)
                .HasColumnName("Address")
                .IsRequired();

            builder
                .Property(sup => sup.Phone)
                .HasColumnName("Phone")
                .IsRequired();

            builder
                .Property(sup => sup.Email)
                .HasColumnName("Email")
                .IsRequired();

            builder
                .Property(sup => sup.WebSite)
                .HasColumnName("WebSite");

            builder
                .Property(sup => sup.ContactPerson)
                .HasColumnName("ContactPerson")
                .IsRequired();

            builder
                .Property(sup => sup.Notes)
                .HasColumnName("Notes");
        }
    }
}