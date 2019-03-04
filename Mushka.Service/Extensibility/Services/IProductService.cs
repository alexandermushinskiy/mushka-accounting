using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.Dto;

namespace Mushka.Service.Extensibility.Services
{
    public interface IProductService : IServiceBase<Product>
    {
        Task<ValidationResponse<IEnumerable<Product>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<Product>>> GetByCriteriaAsync(string criteria, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<Product>>> GetInStockAsync(bool inStock, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<Size>>> GetSizesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<ProductCostPrice>> GetCostPriceAsync(Guid productId, int count, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<ExportedFile>> ExportAsync(string title, IEnumerable<Guid> productIds, CancellationToken cancellationToken = default(CancellationToken));
    }
}