using Autofac;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Repositories;

namespace Mushka.Infrastructure.DataAccess
{
    public class DataAccessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<SizeRepository>().As<ISizeRepository>();
            builder.RegisterType<DeliveryRepository>().As<IDeliveryRepository>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();
        }
    }
}