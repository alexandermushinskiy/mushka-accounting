using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;

namespace Mushka.Service.Extensibility.Services
{
    public interface IServiceBase<TEntity>
    {
        Task<ValidationResponse<TEntity>> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<TEntity>> DeleteAsync(Guid entityId, CancellationToken cancellationToken = default(CancellationToken));
    }
}