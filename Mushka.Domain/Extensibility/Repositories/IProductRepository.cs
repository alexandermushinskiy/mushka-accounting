using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryId(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));

        Task<ProductSize> GetProductSizeAsync(Guid productId, Guid sizeId, CancellationToken cancellationToken);

        Task<ProductSize> UpdateProductSize(ProductSize productSize, CancellationToken cancellationToken);

        Task<IEnumerable<Size>> GetSizesAsync(CancellationToken cancellationToken);
    }
}