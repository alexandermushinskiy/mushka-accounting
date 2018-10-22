using Autofac;
using Mushka.Accounting.Service.Extensibility.Services;
using Mushka.Accounting.Service.Services;

namespace Mushka.Accounting.Service
{
    public class ServiceAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<ProductService>().As<IProductService>();
        }
    }
}