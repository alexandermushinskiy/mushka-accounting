using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;

namespace Mushka.Service.Extensibility.Services
{
    public interface IServiceBase<TEntity>
    {
        Task<OperationResult<TEntity>> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<OperationResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<OperationResult> DeleteAsync(Guid entityId, CancellationToken cancellationToken = default(CancellationToken));
    }
}