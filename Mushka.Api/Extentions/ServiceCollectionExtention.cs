using Microsoft.Extensions.DependencyInjection;
using Mushka.Api.Converters.Categories;
using Mushka.Api.Converters.CorporateOrders;
using Mushka.Api.Converters.Exhibitions;
using Mushka.Api.Converters.Expenses;
using Mushka.Api.Converters.Orders;
using Mushka.Api.Converters.Products;
using Mushka.Api.Converters.Suppliers;
using Mushka.Api.Converters.Supplies;

namespace Mushka.Api.Extentions;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddTransient<IExpenseRequestConverter, ExpenseRequestConverter>();
        services.AddTransient<ICategoryRequestConverter, CategoryRequestConverter>();
        services.AddTransient<IOrderRequestConverter, OrderRequestConverter>();
        services.AddTransient<ICorporateOrderRequestConverter, CorporateOrderRequestConverter>();
        services.AddTransient<ISupplierRequestConverter, SupplierRequestConverter>();
        services.AddTransient<IExhibitionRequestConverter, ExhibitionRequestConverter>();
        services.AddTransient<IProductRequestConverter, ProductRequestConverter>();
        services.AddTransient<ISupplyRequestConverter, SupplyRequestConverter>();

        return services;
    }
}
