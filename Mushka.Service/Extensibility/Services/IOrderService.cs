using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;
using Mushka.Service.Extensibility.Dto;

namespace Mushka.Service.Extensibility.Services
{
    public interface IOrderService : IServiceBase<Order>
    {
        Task<OperationResult<ItemsList<OrderSummaryDto>>> SearchAsync(
            SearchOrdersFilter searchOrdersFilter, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<Order>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<ExportedFile>> ExportAsync(string title, IEnumerable<Guid> orderIds, CancellationToken cancellationToken = default(CancellationToken));
    }
}