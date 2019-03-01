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

        public override async Task<IEnumerable<Product>> GetAsync(
            Expression<Func<Product, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken)) =>
            await dbSet.Where(predicate)
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Size)
                .ToListAsync(cancellationToken);

        public override async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Size)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(prod => prod.Size)
                .FirstOrDefaultAsync(prod => prod.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByCategoryId(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Products
                .AsNoTracking()
                .Where(prod => prod.CategoryId == categoryId)
                .Include(p => p.Category)
                .Include(prod => prod.Size)
                .Include(prod => prod.Supplies)
                    .ThenInclude(del => del.Supply)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<IEnumerable<Size>> GetSizesAsync(CancellationToken cancellationToken) =>
            await Context.Sizes.AsNoTracking().ToListAsync(cancellationToken);

        public async Task<IEnumerable<Product>> GetForExportAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Products
                .AsNoTracking()
                .Where(predicate)
                .Include(prod => prod.Size)
                .Include(prod => prod.Supplies)
                .Include(prod => prod.Orders)
                .ToListAsync(cancellationToken);
        }
    }
}