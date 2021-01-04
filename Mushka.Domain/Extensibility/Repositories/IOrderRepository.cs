using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<IEnumerable<OrderSummaryDto>> SearchAsync(SearchOrdersFilter searchOrdersFilter, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetCountAsync(SearchOrdersFilter searchOrdersFilter, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetSoldProductCount(Guid productId, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<Order>> GetForExportAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    }
}