using System;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Entities;
using Mushka.Infrastructure.DataAccess.Database.Configurations;
using Mushka.Infrastructure.DataAccess.Database.SeedData;

namespace Mushka.Infrastructure.DataAccess.Database
{
    public class MushkaDbContext : DbContext
    {
        public MushkaDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductSize> ProductSizes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SizeConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new ProductSizeConfig());
            modelBuilder.ApplyConfiguration(new DeliveryConfig());
            modelBuilder.ApplyConfiguration(new DeliveryProductConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new OrderProductConfig());
            modelBuilder.ApplyConfiguration(new SupplierConfig());
            modelBuilder.ApplyConfiguration(new ContactPersonConfig());

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new { Id = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"), Name = "Socks" },
                new { Id = Guid.Parse("0E7BE1DE-267C-4C0A-8EE9-ABA0A267F27A"), Name = "Pack" },
                new { Id = Guid.Parse("B425D75B-2E72-45F0-A55D-3BA400051E5F"), Name = "Other" }
            );

            modelBuilder.Entity<Size>().HasData(
                new { Id = Guid.Parse("ECCEF8A9-2C41-4270-9001-D0EB7E21B9E2"), Name = "36-39" },
                new { Id = Guid.Parse("2DFA21EF-5EED-462F-B5E5-06EE31281BA2"), Name = "41-45" },
                new { Id = Guid.Parse("FB8356A5-1629-4F9F-9B51-3D40E0E55F84"), Name = "39-42" },
                new { Id = Guid.Parse("6E519491-8FD8-45F2-992E-270B01F25971"), Name = "43-46" }
            );

            modelBuilder.HasProducts();
            modelBuilder.HasDeliveries(); 
        }
    }
}