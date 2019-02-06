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
    internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(MushkaDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Orders
                .AsNoTracking()
                .Include(order => order.Customer)
                .Include(order => order.Products)
                .ToListAsync(cancellationToken);

        public override async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Orders
                .Where(order => order.Id == id)
                .AsNoTracking()
                .Include(order => order.Customer)
                .Include(order => order.Products)
                    .ThenInclude(prod => prod.Product.Size)
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<int> GetSoldProductCount(Guid productId, CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Set<OrderProduct>()
                .Where(sp => sp.ProductId == productId)
                .SumAsync(sp => sp.Quantity, cancellationToken);
        
        public override Order Update(Order order)
        {
            var storedOrder = dbSet
                .AsNoTracking()
                .Include(o => o.Products)
                .Include(o => o.Customer)
                .Single(o => o.Id == order.Id);

            order.Products
                .ToList()
                .ForEach(op =>
                {
                    Context.Entry(op).State = storedOrder.Products.Any(sop => sop.ProductId == op.ProductId)
                        ? EntityState.Modified
                        : EntityState.Added;
                });

            storedOrder.Products
                .Except(order.Products, new OrderProductComparer())
                .ToList()
                .ForEach(sp => Context.Entry(sp).State = EntityState.Deleted);

            Context.Orders.Update(order);

            return order;
        }
    }
}