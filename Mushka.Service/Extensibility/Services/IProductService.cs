using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface IProductService : IServiceBase<Product>
    {
        Task<OperationResult<IEnumerable<Product>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<Product>>> GetByCriteriaAsync(string criteria, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<Product>>> GetInStockAsync(bool inStock, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<Size>>> GetSizesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<ProductCostPrice>> GetCostPriceAsync(Guid productId, int count, CancellationToken cancellationToken = default(CancellationToken));
    }
}