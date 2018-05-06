using Autofac;
using Mushka.Accounting.WebApi.Extensibility.Providers;
using Mushka.Accounting.WebApi.Providers;

namespace Mushka.Accounting.WebApi
{
    public class WebApiAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActionResultProvider>().As<IActionResultProvider>();
        }
    }
}