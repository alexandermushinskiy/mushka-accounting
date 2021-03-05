using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Comparers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Models;
using Mushka.Infrastructure.DataAccess.Database;
using Mushka.Infrastructure.DataAccess.Extensions;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class SupplyRepository : RepositoryBase<Supply>, ISupplyRepository
    {
        public SupplyRepository(MushkaDbContext context) : base(context)
        {
        }

        public async Task<int> GetAllCountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Supplies.CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<Supply>> GetByFilterAsync(SearchSuppliesFilter searchSuppliesFilter, CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Supplies
                .Include(sup => sup.Supplier)
                .Include(sup => sup.Products)
                .ThenInclude(supProd => supProd.Product)
                .ThenInclude(supProd => supProd.Size)
                .Where(sup => searchSuppliesFilter.SearchKey == null ||
                              (sup.Supplier.Name.ToUpper().Contains(searchSuppliesFilter.SearchKey.ToUpper()) ||
                               sup.Description.ToUpper().Contains(searchSuppliesFilter.SearchKey.ToUpper())))
                .Where(sup => searchSuppliesFilter.ProductId == null ||
                              sup.Products.Any(prod => prod.ProductId == searchSuppliesFilter.ProductId))
                .OrderBy(supply => supply.Supplier.Name)
                .ThenBy(supply => supply.RequestDate)
                .ToListAsync(cancellationToken);

        public override async Task<IEnumerable<Supply>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Supplies
                .Include(sup => sup.Supplier)
                .Include(sup => sup.Products)
                    .ThenInclude(supProd => supProd.Product)
                    .ThenInclude(supProd => supProd.Size)
                .ToListAsync(cancellationToken);

        public virtual async Task<IEnumerable<Supply>> GetAsync(
            Expression<Func<Supply, bool>> predicate,
            string[] includes,
            CancellationToken cancellationToken = default(CancellationToken)) =>
            await dbSet.Where(predicate)
                .IncludeMultiple(includes)
                .AsNoTracking()
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