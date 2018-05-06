using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Domain.Extensibility.Repositories
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Get(Expression<Func<Category, bool>> predicate);

        Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default(CancellationToken));

        Task<Category> UpdateAsync(Category category, CancellationToken cancellationToken = default(CancellationToken));

        Task<Category> DeleteAsync(Category category, CancellationToken cancellationToken = default(CancellationToken));
    }
}