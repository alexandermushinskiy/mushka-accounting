using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<int> GetSoldProductCount(Guid productId, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<Order>> GetForExportAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    }
}