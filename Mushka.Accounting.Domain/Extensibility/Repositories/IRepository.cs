using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mushka.Accounting.Domain.Extensibility.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> AddAsync(TEntity category, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> UpdateAsync(TEntity category, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> DeleteAsync(TEntity category, CancellationToken cancellationToken = default(CancellationToken));
    }
}