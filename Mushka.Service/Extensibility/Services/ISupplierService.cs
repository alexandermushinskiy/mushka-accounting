using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface ISupplierService : IServiceBase<Supplier>
    {
        Task<ValidationResponse<IEnumerable<Supplier>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}