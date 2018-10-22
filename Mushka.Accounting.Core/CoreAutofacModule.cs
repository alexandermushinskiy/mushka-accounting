using Autofac;
using Mushka.Accounting.Core.Extensibility.Logging;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Core.Logging;
using Mushka.Accounting.Core.Providers;

namespace Mushka.Accounting.Core
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