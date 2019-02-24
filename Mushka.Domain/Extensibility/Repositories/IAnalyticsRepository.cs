using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Dto;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IAnalyticsRepository : IRepositoryBase
    {
        Task<Balance> GetBalance(CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<PopularProduct>> GetPopularProducts(int topCount, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<PopularCity>> GetPopularCities(int topCount, CancellationToken cancellationToken = default(CancellationToken));
    }
}