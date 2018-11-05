using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Extensions
{
    public static class DatabaseConfigurationExtensions
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<MushkaDbContext>(options => options.UseSqlServer(connectionString, builder => builder.EnableRetryOnFailure()));
    }
}