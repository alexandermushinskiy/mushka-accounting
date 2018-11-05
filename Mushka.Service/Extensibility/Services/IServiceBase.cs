using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;

namespace Mushka.Service.Extensibility.Services
{
    public interface IServiceBase<TEntity>
    {
        Task<ValidationResponse<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<TEntity>> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<TEntity>> AddAsync(TEntity category, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<TEntity>> UpdateAsync(TEntity category, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<TEntity>> DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));
    }
}