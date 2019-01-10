using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface IProductService : IServiceBase<Product>
    {
        Task<ValidationResponse<IEnumerable<Product>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<Product>>> GetByCriteriaAsync(string criteria, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<Product>>> GetInStockAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<Size>>> GetSizesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<decimal>> GetCostPriceAsync(Guid productId, int count, CancellationToken cancellationToken = default(CancellationToken));
    }
}