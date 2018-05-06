using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mushka.Accounting.Infrastructure.DataAccess.Database;

namespace Mushka.Accounting.Infrastructure.DataAccess.Extensions
{
    public static class DatabaseConfigurationExtensions
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<AccountingDbContext>(options => options.UseSqlServer(connectionString, builder => builder.EnableRetryOnFailure()));
    }
}