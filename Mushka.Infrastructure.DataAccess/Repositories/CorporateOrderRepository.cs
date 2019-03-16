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
    internal class CorporateOrderRepository : RepositoryBase<CorporateOrder>, ICorporateOrderRepository
    {
        public CorporateOrderRepository(MushkaDbContext context) : base(context)
        {
        }
        
        public override async Task<CorporateOrder> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.CorporateOrders
                .Where(order => order.Id == id)
                .AsNoTracking()
                .Include(order => order.Products)
                .FirstOrDefaultAsync(cancellationToken);

        public void DeleteProducts(Guid corporateOrderId)
        {
            Context.Set<CorporateOrderProduct>()
                .Where(prod => prod.CorporateOrderId == corporateOrderId)
                .ToList()
                .ForEach(prod => Context.Entry(prod).State = EntityState.Deleted);
        }
    }
}