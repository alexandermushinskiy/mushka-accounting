using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Infrastructure.DataAccess.Database;

namespace Mushka.Accounting.Infrastructure.DataAccess.Repositories
{
    internal class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(AccountingDbContext context) : base(context)
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