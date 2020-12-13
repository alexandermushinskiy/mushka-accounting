using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface ICorporateOrderService : IServiceBase<CorporateOrder>
    {
        Task<OperationResult<IEnumerable<CorporateOrder>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<OperationResult<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken));
    }
}