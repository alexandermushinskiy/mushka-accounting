using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Infrastructure.DataAccess.Database;

namespace Mushka.Accounting.Infrastructure.DataAccess.Repositories
{
    internal class CategoryRepository : Repository, ICategoryRepository
    {
        public CategoryRepository(AccountingDbContext context) : base(context)
        {
        }

        public IQueryable<Category> Get(Expression<Func<Category, bool>> predicate) =>
            Context.Categories.Where(predicate).AsNoTracking();

        public async Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Categories.Where(cat => cat.Id == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Categories.AsNoTracking().ToArrayAsync(cancellationToken);

        public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            Context.Categories.Add(category);
            await Context.SaveChangesAsync(cancellationToken);
            return category;
        }

        public async Task<Category> UpdateAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            Context.Categories.Update(category);
            await Context.SaveChangesAsync(cancellationToken);
            return category;
        }

        public async Task<Category> DeleteAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            Context.Categories.Remove(category);
            await Context.SaveChangesAsync(cancellationToken);
            return category;
        }
    }
}