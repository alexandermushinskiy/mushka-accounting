using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class SupplyRepository : RepositoryBase<Supply>, ISupplyRepository
    {
        public SupplyRepository(MushkaDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Supply>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Supplies
                .Include(del => del.Supplier)
                .Include(del => del.Products)
                    .ThenInclude(delProd => delProd.Product)
                    .ThenInclude(delProd => delProd.Size)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Supply> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Supplies
                .Where(del => del.Id == id)
                .Include(del => del.Products)
                    .ThenInclude(delProd => delProd.Product)
                .Include(del => del.Products)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}