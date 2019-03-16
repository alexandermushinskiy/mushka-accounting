using Autofac;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;
using Mushka.Service.Providers;
using Mushka.Service.Services;

namespace Mushka.Service
{
    public class ServiceAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<SupplyService>().As<ISupplyService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<SupplierService>().As<ISupplierService>();
            builder.RegisterType<ExhibitionService>().As<IExhibitionService>();
            builder.RegisterType<ExpenseService>().As<IExpenseService>();
            builder.RegisterType<AnalyticsService>().As<IAnalyticsService>();
            builder.RegisterType<CorporateOrderService>().As<ICorporateOrderService>();

            builder.RegisterType<CostPriceProvider>().As<ICostPriceProvider>();
            builder.RegisterType<DefaultProductsProvider>().As<IDefaultProductsProvider>();
        }
    }
}