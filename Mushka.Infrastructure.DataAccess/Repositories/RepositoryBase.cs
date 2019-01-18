using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly DbSet<TEntity> dbSet;

        protected RepositoryBase(MushkaDbContext context)
        {
            Context = context;

            dbSet = context.Set<TEntity>();
        }

        protected MushkaDbContext Context { get; }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken)) =>
                await dbSet.Where(predicate)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken)) =>
                await dbSet.Where(predicate).AsNoTracking()
                    .AnyAsync(cancellationToken);
        
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate) =>
            dbSet.Where(predicate).AsNoTracking();

        public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await dbSet.Where(entity => entity.Id == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            await dbSet.AsNoTracking().ToArrayAsync(cancellationToken);
        
        public virtual TEntity Add(TEntity entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            dbSet.Update(entity);
            return entity;
        }

        public virtual TEntity Delete(TEntity entity)
        {
            dbSet.Remove(entity);
            return entity;
        }
    }
}