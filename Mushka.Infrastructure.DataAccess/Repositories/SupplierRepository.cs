using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Comparers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
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

        public override async Task<Supplier> UpdateAsync(Supplier supplier, CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedContactPersons = await Context.Suppliers
                .Where(sup => sup.Id == supplier.Id)
                .SelectMany(sup => sup.ContactPersons)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            supplier.ContactPersons
                .ToList()
                .ForEach(cp => Context.Entry(cp).State = storedContactPersons.Any(scp => scp.Id == cp.Id) ? EntityState.Modified : EntityState.Added);
            
            storedContactPersons
                .Except(supplier.ContactPersons, new EntityComparer<ContactPerson>())
                .ToList()
                .ForEach(dcp => Context.Entry(dcp).State = EntityState.Deleted);
            
            Context.Suppliers.Update(supplier);
            await Context.SaveChangesAsync(cancellationToken);

            return supplier;
        }
    }
}