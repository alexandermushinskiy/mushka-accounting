using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Dto;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IAnalyticsRepository : IRepositoryBase
    {
        Task<Balance> GetBalance(CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<PopularProduct>> GetProductsByPopularity(int topCount, Popularity popularity, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<IEnumerable<PopularCity>> GetPopularCities(int topCount, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<OrdersCount>> GetOrdersCount(DateTime limitDate, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<SoldProductsCount>> GetSoldProductsCount(DateTime limitDate, CancellationToken cancellationToken = default(CancellationToken));
    }
}