using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface ICorporateOrderService : IServiceBase<CorporateOrder>
    {
        Task<ValidationResponse<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken));
    }
}