using Autofac;
using AutoMapper;
using Mushka.WebApi.Extensibility.Providers;
using Mushka.WebApi.Providers;
using System.Linq;

namespace Mushka.WebApi
{
    public class WebApiAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActionResultProvider>().As<IActionResultProvider>();

            // Register all AutoMapper converters/resolvers from this assembly
            builder.RegisterAssemblyTypes(typeof(WebApiAutofacModule).Assembly)
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(ITypeConverter<,>) ||
                     i.GetGenericTypeDefinition() == typeof(IValueResolver<,,>) ||
                     i.GetGenericTypeDefinition() == typeof(IMemberValueResolver<,,,>))))
                .AsSelf();
        }
    }
}