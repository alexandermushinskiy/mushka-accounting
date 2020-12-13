using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface ICategoryService : IServiceBase<Category>
    {
        Task<OperationResult<IEnumerable<Category>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}