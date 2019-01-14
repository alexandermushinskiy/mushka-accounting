using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface IOrderService : IServiceBase<Order>
    {
        Task<ValidationResponse<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<OrderProduct>>> GetDefaultProducts(CancellationToken cancellationToken = default(CancellationToken));
    }
}