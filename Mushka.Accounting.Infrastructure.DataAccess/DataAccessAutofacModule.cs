using Autofac;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Infrastructure.DataAccess.Repositories;

namespace Mushka.Accounting.Infrastructure.DataAccess
{
    public class DataAccessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<SizeRepository>().As<ISizeRepository>();
        }
    }
}