﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder
                .Property(order => order.Id)
                .HasColumnName("Id");

            builder.HasKey(order => order.Id);

            builder
                .Property(order => order.OrderDate)
                .HasColumnName("OrderDate")
                .IsRequired();
        }
    }
}