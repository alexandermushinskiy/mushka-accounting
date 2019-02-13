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
            builder.RegisterType<SupplyRepository>().As<ISupplyRepository>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();
            builder.RegisterType<SupplierRepository>().As<ISupplierRepository>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<ExhibitionRepository>().As<IExhibitionRepository>();

            builder.RegisterType<Storage>().As<IStorage>();
        }
    }
}