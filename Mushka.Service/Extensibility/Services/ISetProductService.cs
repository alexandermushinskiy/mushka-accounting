using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface ISetProductService
    {
        Task<ValidationResponse<IEnumerable<Set>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<Set>> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default(CancellationToken));
    }
}