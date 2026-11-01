using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;

namespace Mushka.Service.Extensibility.Services
{
    public interface IOrderService : IServiceBase<Order>
    {
        Task<OperationResult<ItemsList<OrderSummaryDto>>> SearchAsync(
            SearchOrdersFilter searchOrdersFilter, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<Order>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken));
    }
}