using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(MushkaDbContext context) : base(context)
        {
        }
        
        public override async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Products
                .AsNoTracking()
                .Include(p => p.Sizes)
                    .ThenInclude(s => s.Size)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Products
                .AsNoTracking()
                .Include(prod => prod.Sizes)
                    .ThenInclude(s => s.Size)
                .FirstOrDefaultAsync(prod => prod.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByCategoryId(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Products
                .AsNoTracking()
                .Where(prod => prod.CategoryId == categoryId)
                .Include(prod => prod.Sizes)
                    .ThenInclude(s => s.Size)
                .Include(prod => prod.Deliveries)
                    .ThenInclude(del => del.Delivery)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductSize> GetProductSizeAsync(Guid productId, Guid sizeId, CancellationToken cancellationToken)
        {
            return await Context.ProductSizes
                .AsNoTracking()
                .FirstOrDefaultAsync(prod => prod.ProductId == productId && prod.SizeId == sizeId, cancellationToken);
        }

        public async Task<ProductSize> UpdateProductSize(ProductSize productSize, CancellationToken cancellationToken)
        {
            Context.Entry(productSize).State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken);
            return productSize;
        }

        public async Task<IEnumerable<Size>> GetSizesAsync(CancellationToken cancellationToken)
        {
            return await Context.Sizes.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}