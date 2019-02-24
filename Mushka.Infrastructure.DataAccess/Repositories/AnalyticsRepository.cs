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
    }
}