using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface ICustomerService
    {
        Task<OperationResult<IEnumerable<Customer>>> GetByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}