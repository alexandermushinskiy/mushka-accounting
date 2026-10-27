using System.Threading;
using System.Threading.Tasks;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IHealthRepository : IRepositoryBase
    {
        Task<bool> CanConnectAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
