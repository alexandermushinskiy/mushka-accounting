using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class SetProductService : ISetProductService
    {
        public SetProductService()
        {
            
        }

        public Task<ValidationResponse<IEnumerable<Set>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResponse<Set>> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}