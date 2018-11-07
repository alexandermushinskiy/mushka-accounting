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
    internal class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        public SupplierRepository(MushkaDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Supplier>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Suppliers
                .Include(sup => sup.ContactPersons)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public override async Task<Supplier> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Suppliers
                .Include(sup => sup.ContactPersons)
                .Where(entity => entity.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}