using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface ISupplyRepository : IRepositoryBase<Supply>
    {
        Task<IEnumerable<Supply>> GetByFilterAsync(SearchSuppliesFilter searchSuppliesFilter, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetAllCountAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<SupplyProduct>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<Supply>> GetAsync(Expression<Func<Supply, bool>> predicate, string[] includes, CancellationToken cancellationToken = default(CancellationToken));
    }
}