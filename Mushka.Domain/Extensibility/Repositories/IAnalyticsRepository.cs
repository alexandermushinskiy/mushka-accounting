using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Dto;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IAnalyticsRepository : IRepositoryBase
    {
        Task<IEnumerable<PopularProduct>> GetPopularProducts(int topCount, CancellationToken cancellationToken = default(CancellationToken));
    }
}