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
    internal class DeliveryRepository : RepositoryBase<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(MushkaDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Delivery>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Deliveries
                .Include(del => del.Products)
                    .ThenInclude(delProd => delProd.Product)
                .Include(del => del.Products)
                    .ThenInclude(delProd => delProd.ProductSizes)
                        //.ThenInclude(ps => ps.Size)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Delivery> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Deliveries
                .Where(del => del.Id == id)
                .Include(del => del.Products)
                    .ThenInclude(delProd => delProd.Product)
                .Include(del => del.Products)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}