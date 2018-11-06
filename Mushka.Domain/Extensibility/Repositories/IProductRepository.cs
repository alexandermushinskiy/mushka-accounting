using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<ProductSize> GetProductSizeAsync(Guid productId, Guid sizeId, CancellationToken cancellationToken);

        Task<ProductSize> UpdateProductSize(ProductSize productSize, CancellationToken cancellationToken);
    }
}