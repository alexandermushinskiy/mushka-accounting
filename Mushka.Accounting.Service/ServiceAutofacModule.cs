using Autofac;
using Mushka.Accounting.Service.Extensibility;

namespace Mushka.Accounting.Service
{
    public class ServiceAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<SupplierService>().As<ISupplierService>();
        }
    }
}