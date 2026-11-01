using Microsoft.Extensions.DependencyInjection;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Logging;
using Mushka.Core.Providers;

namespace Mushka.Core.Extentions
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddTransient<ILoggerFactory, LoggerFactory>();
            services.AddTransient<ICancellationTokenSourceProvider, CancellationTokenSourceProvider>();
            services.AddTransient<IGuidProvider, GuidProvider>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
