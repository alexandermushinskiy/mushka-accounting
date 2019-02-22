using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Entities;
using Mushka.Infrastructure.DataAccess.Database.Configurations;

namespace Mushka.Infrastructure.DataAccess.Database
{
    public class MushkaDbContext : DbContext
    {
        public MushkaDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Size> Sizes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Supply> Supplies { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Exhibition> Exhibitions { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SizeConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new SupplyConfig());
            modelBuilder.ApplyConfiguration(new SupplyProductConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new OrderProductConfig());
            modelBuilder.ApplyConfiguration(new SupplierConfig());
            modelBuilder.ApplyConfiguration(new ContactPersonConfig());
            modelBuilder.ApplyConfiguration(new PaymentCardConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new ExhibitionConfig());
            modelBuilder.ApplyConfiguration(new ExhibitionProductConfig());
            modelBuilder.ApplyConfiguration(new ExpenseConfig());
        }
    }
}