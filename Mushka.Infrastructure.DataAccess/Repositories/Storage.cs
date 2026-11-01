using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class Storage : IStorage, IDisposable
    {
        private readonly MushkaDbContext dbContext;
        private readonly IServiceProvider serviceProvider;
        private bool isDisposed;

        public Storage(
            MushkaDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            this.dbContext = dbContext;
            this.serviceProvider = serviceProvider;
        }

        public TRepository GetRepository<TRepository>() where TRepository : IRepositoryBase
        {
            return serviceProvider.GetRequiredService<TRepository>();
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