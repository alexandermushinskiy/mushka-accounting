using System.Threading;
using System.Threading.Tasks;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IStorage
    {
        TRepository GetRepository<TRepository>() where TRepository : IRepositoryBase;

        Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}