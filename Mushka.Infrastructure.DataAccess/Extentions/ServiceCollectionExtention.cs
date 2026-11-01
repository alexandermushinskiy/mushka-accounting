using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Extensions;
using Mushka.Infrastructure.DataAccess.Repositories;

namespace Mushka.Infrastructure.DataAccess.Extentions
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration.GetConnectionString("MushkaDb"));

            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ISizeRepository, SizeRepository>();
            services.AddTransient<ISupplyRepository, SupplyRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IExhibitionRepository, ExhibitionRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IAnalyticsRepository, AnalyticsRepository>();
            services.AddTransient<ICorporateOrderRepository, CorporateOrderRepository>();
            services.AddTransient<IHealthRepository, HealthRepository>();

            services.AddTransient<IStorage, Storage>();

            return services;
        }
    }
}
