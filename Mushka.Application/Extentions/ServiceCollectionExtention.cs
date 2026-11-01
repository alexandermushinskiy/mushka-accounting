using Microsoft.Extensions.DependencyInjection;
using Mushka.Application.Analytics.Converters;
using Mushka.Application.Analytics.Providers;
using Mushka.Application.Categories.Converters;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Application.Customers.Converters;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Application.Expenses.Converters;
using Mushka.Application.Orders.Converters;
using Mushka.Application.Orders.Providers;
using Mushka.Application.Products.Converters;
using Mushka.Application.Providers;
using Mushka.Application.Suppliers.Converters;
using Mushka.Application.Supplies.Converters;

namespace Mushka.Application.Extentions;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAnalyticsPeriodProvider, AnalyticsPeriodProvider>();
        services.AddTransient<IAnalyticConverter, AnalyticConverter>();

        services.AddTransient<ICreateExpenseConverter, CreateExpenseConverter>();
        services.AddTransient<IUpdateExpenseConverter, UpdateExpenseConverter>();
        services.AddTransient<ISearchExpensesConverter, SearchExpensesConverter>();
        services.AddTransient<IDescribeExpenseConverter, DescribeExpenseConverter>();

        services.AddTransient<ICreateCategoryConverter, CreateCategoryConverter>();
        services.AddTransient<IUpdateCategoryConverter, UpdateCategoryConverter>();
        services.AddTransient<ISearchCategoriesConverter, SearchCategoriesConverter>();
        services.AddTransient<IDescribeCategoryConverter, DescribeCategoryConverter>();

        services.AddTransient<IGetCustomersByNameConverter, GetCustomersByNameConverter>();

        services.AddTransient<ICreateOrderConverter, CreateOrderConverter>();
        services.AddTransient<IUpdateOrderConverter, UpdateOrderConverter>();
        services.AddTransient<ISearchOrdersConverter, SearchOrdersConverter>();
        services.AddTransient<IDescribeOrderConverter, DescribeOrderConverter>();
        services.AddTransient<IGetDefaultOrderProductsConverter, GetDefaultOrderProductsConverter>();
        services.AddTransient<IOrderCustomerProvider, OrderCustomerProvider>();

        services.AddTransient<ICreateCorporateOrderConverter, CreateCorporateOrderConverter>();
        services.AddTransient<IUpdateCorporateOrderConverter, UpdateCorporateOrderConverter>();
        services.AddTransient<ISearchCorporateOrdersConverter, SearchCorporateOrdersConverter>();
        services.AddTransient<IDescribeCorporateOrderConverter, DescribeCorporateOrderConverter>();

        services.AddTransient<ICreateSupplierConverter, CreateSupplierConverter>();
        services.AddTransient<IUpdateSupplierConverter, UpdateSupplierConverter>();
        services.AddTransient<ISearchSuppliersConverter, SearchSuppliersConverter>();
        services.AddTransient<IDescribeSupplierConverter, DescribeSupplierConverter>();

        services.AddTransient<ICreateExhibitionConverter, CreateExhibitionConverter>();
        services.AddTransient<IUpdateExhibitionConverter, UpdateExhibitionConverter>();
        services.AddTransient<ISearchExhibitionsConverter, SearchExhibitionsConverter>();
        services.AddTransient<IDescribeExhibitionConverter, DescribeExhibitionConverter>();
        services.AddTransient<IGetDefaultExhibitionProductsConverter, GetDefaultExhibitionProductsConverter>();
        services.AddTransient<ICostPriceProvider, CostPriceProvider>();

        services.AddTransient<ICreateProductConverter, CreateProductConverter>();
        services.AddTransient<IUpdateProductConverter, UpdateProductConverter>();
        services.AddTransient<IDescribeProductConverter, DescribeProductConverter>();
        services.AddTransient<IGetProductsByCategoryConverter, GetProductsByCategoryConverter>();
        services.AddTransient<ISelectProductsConverter, SelectProductsConverter>();
        services.AddTransient<IGetProductSizesConverter, GetProductSizesConverter>();

        services.AddTransient<ICreateSupplyConverter, CreateSupplyConverter>();
        services.AddTransient<IUpdateSupplyConverter, UpdateSupplyConverter>();
        services.AddTransient<ISearchSuppliesConverter, SearchSuppliesConverter>();
        services.AddTransient<IDescribeSupplyConverter, DescribeSupplyConverter>();

        return services;
    }
}
