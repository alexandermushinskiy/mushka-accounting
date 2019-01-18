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
    internal class SupplyRepository : RepositoryBase<Supply>, ISupplyRepository
    {
        public SupplyRepository(MushkaDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Supply>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Supplies
                .Include(sup => sup.Supplier)
                .Include(sup => sup.Products)
                    .ThenInclude(supProd => supProd.Product)
                    .ThenInclude(supProd => supProd.Size)
                .ToListAsync(cancellationToken);

        public override async Task<Supply> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Supplies
                .AsNoTracking()
                .Where(sup => sup.Id == id)
                .Include(sup => sup.Products)
                    .ThenInclude(supProd => supProd.Product)
                    .ThenInclude(supProd => supProd.Size)
                .Include(sup => sup.Products)
                .FirstOrDefaultAsync(cancellationToken);
        
        public virtual async Task<IEnumerable<SupplyProduct>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Set<SupplyProduct>()
                .Where(sp => sp.ProductId == productId)
                .OrderBy(sp => sp.Supply.ReceivedDate)
                .ToListAsync(cancellationToken);

        public override Supply Update(Supply supply)
        {
            var storedSupply = Context.Supplies
                .AsNoTracking()
                .Include(sup => sup.Products)
                .Single(sup => sup.Id == supply.Id);

            supply.Products
                .ToList()
                .ForEach(sp =>
                {
                    Context.Entry(sp).State = storedSupply.Products.Any(spc => spc.ProductId == sp.ProductId)
                            ? EntityState.Modified
                            : EntityState.Added;
                });

            storedSupply.Products
                .Except(supply.Products, new SupplyProductComparer())
                .ToList()
                .ForEach(sp => Context.Entry(sp).State = EntityState.Deleted);

            Context.Supplies.Update(supply);

            return supply;
        }
    }
}