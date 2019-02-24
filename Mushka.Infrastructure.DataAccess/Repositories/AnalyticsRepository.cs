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
        public AnalyticsRepository(MushkaDbContext context) : base(context)
        {
        }

        public async Task<Balance> GetBalance(CancellationToken cancellationToken = default(CancellationToken))
        {
            var profit = await Context.Orders.SumAsync(order => order.Profit, cancellationToken);

            var supplyExpenses = await Context.Supplies.SumAsync(supply => supply.TotalCost, cancellationToken);
            var expenses = await Context.Expenses.SumAsync(expense => expense.Cost, cancellationToken);

            return new Balance(supplyExpenses + expenses, profit);
        }

        public async Task<IEnumerable<PopularProduct>> GetPopularProducts(int topCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await Context.Set<OrderProduct>()
                .Where(op => op.Product.CategoryId == Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C"))
                .Include(op => op.Product.Size)
                .GroupBy(op => op.Product)
                .Select(res => new { Product = res.Key, Count = res.Count() })
                .OrderByDescending(res => res.Count)
                .Take(topCount)
                .ToListAsync(cancellationToken);

            return result
                .Select(res => new PopularProduct(res.Product.Name, res.Product.Size.Name, res.Product.VendorCode, res.Count));
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
    }
}