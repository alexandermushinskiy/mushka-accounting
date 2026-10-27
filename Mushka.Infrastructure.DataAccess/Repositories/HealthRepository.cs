using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class HealthRepository : RepositoryBase, IHealthRepository
    {
        public HealthRepository(MushkaDbContext context)
            : base(context)
        {
        }

        public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            Context.Database.CanConnectAsync(cancellationToken);
    }
}
