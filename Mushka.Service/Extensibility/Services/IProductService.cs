using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface IProductService : IServiceBase<Product>
    {
        Task<ValidationResponse<IEnumerable<Product>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));
    }
}