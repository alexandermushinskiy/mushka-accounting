using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Accounting.Domain.Extensibility.Entities;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Infrastructure.DataAccess.Database;

namespace Mushka.Accounting.Infrastructure.DataAccess.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> dbSet;

        protected RepositoryBase(AccountingDbContext context)
        {
            Context = context;

            dbSet = context.Set<TEntity>();
        }

        protected AccountingDbContext Context { get; }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate) =>
            dbSet.Where(predicate).AsNoTracking();

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await dbSet.Where(entity => entity.Id == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            await dbSet.AsNoTracking().ToArrayAsync(cancellationToken);

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            dbSet.Add(entity);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            dbSet.Update(entity);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            dbSet.Remove(entity);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}