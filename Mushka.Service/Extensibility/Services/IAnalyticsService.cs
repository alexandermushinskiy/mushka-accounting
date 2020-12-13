using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;

namespace Mushka.Service.Extensibility.Services
{
    public interface IAnalyticsService
    {
        Task<OperationResult<Balance>> GetBalance(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<PopularProduct>>> GetPopularProducts(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<PopularProduct>>> GetUnpopularProducts(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<PopularCity>>> GetPopularCities(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<OrdersCount>>> GetOrdersCount(int periodInMonth, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<SoldProductsCount>>> GetSoldProductsCount(int periodInMonth, CancellationToken cancellationToken = default(CancellationToken));
    }
}