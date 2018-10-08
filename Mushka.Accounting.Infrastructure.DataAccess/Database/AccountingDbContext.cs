using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Infrastructure.DataAccess.Database.Configurations;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database
{
    public class AccountingDbContext : DbContext
    {
        public AccountingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureCategory();
            modelBuilder.ConfigureProduct();
            modelBuilder.ConfigureSupplier();
            modelBuilder.ConfigureDelivery();
            modelBuilder.ConfigureDeliveryProduct();
            modelBuilder.ConfigureSizeItem();
            //modelBuilder.ConfigureWarehouse();

            base.OnModelCreating(modelBuilder);
        }
    }
}