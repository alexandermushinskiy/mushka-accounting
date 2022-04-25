using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Comparers;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Models;
using Mushka.Infrastructure.DataAccess.Database;
using Mushka.Infrastructure.DataAccess.Extensions;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(MushkaDbContext context) : base(context)
        {
        }

        public async Task<int> GetCountAsync(
            SearchOrdersFilter searchOrdersFilter,
            CancellationToken cancellationToken)
        {
            var query = Context.Orders
                .AsNoTracking()
                .Include(order => order.Customer)
                .Where(order => searchOrdersFilter.OrderDate.From == null || (order.OrderDate >= searchOrdersFilter.OrderDate.From))
                .Where(order => searchOrdersFilter.OrderDate.To == null || (order.OrderDate <= searchOrdersFilter.OrderDate.To))
                .Where(order => String.IsNullOrEmpty(searchOrdersFilter.SearchKey) ||
                                    EF.Functions.Like(order.Customer.FullName, $"%{searchOrdersFilter.SearchKey}%") ||
                                    EF.Functions.Like(order.Number, $"%{searchOrdersFilter.SearchKey}%"));

            return await query.CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<OrderSummaryDto>> SearchAsync(
            SearchOrdersFilter searchOrdersFilter,
            CancellationToken cancellationToken)
        {
            var query = Context.Orders
                .AsNoTracking()
                .Include(order => order.Customer)
                .Where(order => searchOrdersFilter.OrderDate.From == null || (order.OrderDate >= searchOrdersFilter.OrderDate.From))
                .Where(order => searchOrdersFilter.OrderDate.To == null || (order.OrderDate <= searchOrdersFilter.OrderDate.To))
                .Where(order => String.IsNullOrEmpty(searchOrdersFilter.SearchKey) ||
                                    EF.Functions.Like(order.Customer.FullName, $"%{searchOrdersFilter.SearchKey}%") ||
                                    EF.Functions.Like(order.Number, $"%{searchOrdersFilter.SearchKey}%"));

            return await query
                .Select(order => new OrderSummaryDto
                    {
                        Id = order.Id,
                        OrderDate = order.OrderDate,
                        OrderNumber = order.Number,
                        Cost = order.Cost,
                        Address = order.Address,
                        CustomerName = order.Customer.FullName,
                        ProductsCount = order.Products.Sum(p => p.Quantity),
                        IsWholesale = order.IsWholesale
                    })
                .OrderBy(searchOrdersFilter.SortKey, searchOrdersFilter.IsAsc)
                .Skip(searchOrdersFilter.CurrentPage * searchOrdersFilter.PageSize)
                .Take(searchOrdersFilter.PageSize)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken) =>
            await Context.Orders
                .AsNoTracking()
                .Include(order => order.Customer)
                .Include(order => order.Products)
                .ToListAsync(cancellationToken);

        public override async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await Context.Orders
                .Where(order => order.Id == id)
                .AsNoTracking()
                .Include(order => order.Customer)
                .Include(order => order.Products)
                    .ThenInclude(prod => prod.Product.Size)
                .Include(order => order.Products)
                    .ThenInclude(prod => prod.Product.Category)
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<int> GetSoldProductCount(Guid productId, CancellationToken cancellationToken) =>
            await Context.Set<OrderProduct>()
                .Where(sp => sp.ProductId == productId)
                .SumAsync(sp => sp.Quantity, cancellationToken);

        public async Task<IEnumerable<Order>> GetForExportAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Context.Orders
                .Where(predicate)
                .AsNoTracking()
                .Include(order => order.Products)
                    .ThenInclude(prod => prod.Product.Size)
                .ToListAsync(cancellationToken);
        }

        public override Order Add(Order order)
        {
            Context.Entry(order.Customer).State = Context.Customers.Any(cust => cust.Phone == order.Customer.Phone)
                ? EntityState.Modified
                : EntityState.Added;
            
            return base.Add(order);
        }

        public override Order Update(Order order)
        {
            var storedOrder = dbSet
                .AsNoTracking()
                .Include(o => o.Products)
                //.Include(o => o.Customer)
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