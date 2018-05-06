using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Infrastructure.DataAccess.Database
{
    public class AccountingDbContext : DbContext
    {
        public AccountingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureView();

            base.OnModelCreating(modelBuilder);
        }
    }
}