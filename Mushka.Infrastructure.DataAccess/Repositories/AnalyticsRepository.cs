using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class AnalyticsRepository : RepositoryBase, IAnalyticsRepository
    {
        private static readonly Guid SocksCategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C");
        private const int PopularityMinSoldCount = 15;

        public AnalyticsRepository(MushkaDbContext context) : base(context)
        {
        }

        public async Task<Balance> GetBalance(CancellationToken cancellationToken = default(CancellationToken))
        {
            var ordersProfit = await Context.Orders.SumAsync(order => order.Profit, cancellationToken);
            var corporateOrdersProfit = await Context.CorporateOrders.SumAsync(order => order.Profit, cancellationToken);

            var supplyExpenses = await Context.Supplies.SumAsync(supply => supply.TotalCost, cancellationToken);
            var expenses = await Context.Expenses.SumAsync(expense => expense.Cost, cancellationToken);

            return new Balance(Convert.ToInt32(supplyExpenses + expenses), Convert.ToInt32(ordersProfit + corporateOrdersProfit));
        }

        public async Task<IEnumerable<PopularProduct>> GetProductsByPopularity(int topCount, Popularity popularity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Context.Set<OrderProduct>()
                .Where(op => op.Product.CategoryId == SocksCategoryId)
                .Include(op => op.Product.Size)
                .GroupBy(op => op.Product)
                .Select(res => new { Product = res.Key, SoldQuantity = res.Sum(x => x.Quantity) })
                .Where(res => res.SoldQuantity > PopularityMinSoldCount);

            query = popularity == Popularity.Popular ? query.OrderByDescending(res => res.SoldQuantity) : query.OrderBy(res => res.SoldQuantity);
            
            var result = await query.Take(topCount)
                .ToListAsync(cancellationToken);

            return result
                .Select(res => new PopularProduct(res.Product.Name, res.Product.Size.Name, res.Product.VendorCode, res.SoldQuantity));
        }

        public async Task<IEnumerable<PopularCity>> GetPopularCities(int topCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await Context.Orders
                .GroupBy(op => op.Customer.City)
                .Select(res => new { City = res.Key, Count = res.Count() })
                .OrderByDescending(res => res.Count)
                .Take(topCount)
                .ToListAsync(cancellationToken);

            return result.Select(res => new PopularCity(res.City, res.Count));
        }

        public async Task<IEnumerable<OrdersCount>> GetOrdersCount(DateTime limitDate, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await Context.Orders
                .Where(order => order.OrderDate > limitDate)
                .GroupBy(order => new { order.OrderDate.Year, order.OrderDate.Month })
                .Select(res => new { OrderDate = res.Key, Count = res.Count() })
                .ToListAsync(cancellationToken);

            return result
                .Select(res => new OrdersCount(new DateTime(res.OrderDate.Year, res.OrderDate.Month, 1), res.Count))
                .OrderBy(res => res.CreatedOn);
        }

        public async Task<IEnumerable<SoldProductsCount>> GetSoldProductsCount(DateTime limitDate, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await Context.Orders
                .Where(order => order.OrderDate > limitDate)
                .GroupBy(order => new { order.OrderDate.Year, order.OrderDate.Month })
                .Select(res => new
                {
                    OrderDate = res.Key,
                    ProductsCount = res.SelectMany(r => r.Products)
                                       .Where(p => p.Product.CategoryId == SocksCategoryId)
                                       .Sum(p => p.Quantity)
                })
                .ToListAsync(cancellationToken);

            return result
                .Select(res => new SoldProductsCount(new DateTime(res.OrderDate.Year, res.OrderDate.Month, 1), res.ProductsCount))
                .OrderBy(res => res.CreatedOn);
        }
    }
}