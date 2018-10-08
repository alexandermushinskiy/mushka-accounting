using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Service.Extensibility.Services
{
    public interface IProductService : IServiceBase<Product>
    {
        Task<ValidationResponse<IEnumerable<Product>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));
    }
}