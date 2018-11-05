using Autofac;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Logging;
using Mushka.Core.Providers;

namespace Mushka.Core
{
    public class CoreAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>();
            builder.RegisterType<CancellationTokenSourceProvider>().As<ICancellationTokenSourceProvider>();
            builder.RegisterType<GuidProvider>().As<IGuidProvider>();
            builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>();
        }
    }
}