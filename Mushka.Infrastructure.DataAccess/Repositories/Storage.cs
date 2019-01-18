using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class Storage : IStorage, IDisposable
    {
        private readonly MushkaDbContext dbContext;
        private readonly ILifetimeScope lifetimeScope;
        private bool isDisposed;

        public Storage(
            MushkaDbContext dbContext,
            ILifetimeScope lifetimeScope)
        {
            this.dbContext = dbContext;
            this.lifetimeScope = lifetimeScope;
        }

        public TRepository GetRepository<TRepository>() where TRepository : IRepositoryBase
        {
            return lifetimeScope.Resolve<TRepository>(new TypedParameter(typeof(MushkaDbContext), dbContext));
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if ( disposing )
                {
                    dbContext.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}