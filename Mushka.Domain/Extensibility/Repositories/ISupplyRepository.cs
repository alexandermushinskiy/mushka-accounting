using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface ISupplyRepository : IRepositoryBase<Supply>
    {
        Task<IEnumerable<SupplyProduct>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken));
    }
}