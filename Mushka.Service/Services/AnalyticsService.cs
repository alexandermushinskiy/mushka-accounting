using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class AnalyticsService : ServiceBase, IAnalyticsService
    {
        private const int DefaultTopCount = 7;
        private readonly IAnalyticsRepository analyticsRepository;

        public AnalyticsService(
            IStorage storage,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            analyticsRepository = storage.GetRepository<IAnalyticsRepository>();
        }

        public async Task<OperationResult<IEnumerable<PopularProduct>>> GetPopularProducts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var popularProducts = await analyticsRepository.GetProductsByPopularity(DefaultTopCount, Popularity.Popular, cancellationToken);

            return OperationResult<IEnumerable<PopularProduct>>.FromResult(popularProducts);
        }

        public async Task<OperationResult<IEnumerable<PopularProduct>>> GetUnpopularProducts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var popularProducts = await analyticsRepository.GetProductsByPopularity(DefaultTopCount, Popularity.Unpopular, cancellationToken);

            return OperationResult<IEnumerable<PopularProduct>>.FromResult(popularProducts);
        }

        public async Task<OperationResult<IEnumerable<PopularCity>>> GetPopularCities(CancellationToken cancellationToken = default(CancellationToken))
        {
            var popularCities = await analyticsRepository.GetPopularCities(DefaultTopCount, cancellationToken);

            return OperationResult<IEnumerable<PopularCity>>.FromResult(popularCities);
        }

        public async Task<OperationResult<Balance>> GetBalance(CancellationToken cancellationToken = default(CancellationToken))
        {
            var balance = await analyticsRepository.GetBalance(cancellationToken);

            return OperationResult<Balance>.FromResult(balance);
        }

        public async Task<OperationResult<IEnumerable<OrdersCount>>> GetOrdersCount(int periodInMonth, CancellationToken cancellationToken = default(CancellationToken))
        {
            var limitDate = GetDateTimeByPeriod(periodInMonth);

            var ordersCount = await analyticsRepository.GetOrdersCount(new DateTime(limitDate.Year, limitDate.Month, 1), cancellationToken);

            return OperationResult<IEnumerable<OrdersCount>>.FromResult(ordersCount);
        }

        public async Task<OperationResult<IEnumerable<SoldProductsCount>>> GetSoldProductsCount(int periodInMonth, CancellationToken cancellationToken = default(CancellationToken))
        {
            var limitDate = GetDateTimeByPeriod(periodInMonth);

            var soldProductsCount = await analyticsRepository.GetSoldProductsCount(new DateTime(limitDate.Year, limitDate.Month, 1), cancellationToken);

            return OperationResult<IEnumerable<SoldProductsCount>>.FromResult(soldProductsCount);
        }

        private static DateTime GetDateTimeByPeriod(int periodInMonth) =>
            DateTime.Now.AddMonths(-periodInMonth + 1);
    }
}