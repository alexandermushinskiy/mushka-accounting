using Autofac;
using Mushka.WebApi.Extensibility.Providers;
using Mushka.WebApi.Providers;

namespace Mushka.WebApi
{
    public class WebApiAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActionResultProvider>().As<IActionResultProvider>();
        }
    }
}