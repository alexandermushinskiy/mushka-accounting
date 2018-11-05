using System.Collections.Generic;
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
    }
}