using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));


        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    }
}