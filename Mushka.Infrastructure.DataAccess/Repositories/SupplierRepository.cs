using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Comparers;
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
                .Include(sup => sup.Supplies)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public override async Task<Supplier> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Suppliers
                .Include(sup => sup.ContactPersons)
                .Include(sup => sup.PaymentCards)
                .Where(entity => entity.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public override Supplier Update(Supplier supplier)
        {
            var storedSupplier = Context.Suppliers
                .AsNoTracking()
                .Include(sup => sup.ContactPersons)
                .Include(sup => sup.PaymentCards)
                .Single(entity => entity.Id == supplier.Id);
            
            supplier.ContactPersons
                .ToList()
                .ForEach(cp => Context.Entry(cp).State = storedSupplier.ContactPersons.Any(scp => scp.Id == cp.Id) ? EntityState.Modified : EntityState.Added);

            storedSupplier.ContactPersons
                .Except(supplier.ContactPersons, new EntityComparer<ContactPerson>())
                .ToList()
                .ForEach(dcp => Context.Entry(dcp).State = EntityState.Deleted);
            
            supplier.PaymentCards
                .ToList()
                .ForEach(cp => Context.Entry(cp).State = storedSupplier.PaymentCards.Any(spc => spc.Id == cp.Id) ? EntityState.Modified : EntityState.Added);

            storedSupplier.PaymentCards
                .Except(supplier.PaymentCards, new EntityComparer<PaymentCard>())
                .ToList()
                .ForEach(dpc => Context.Entry(dpc).State = EntityState.Deleted);

            Context.Suppliers.Update(supplier);

            return supplier;
        }
    }
}