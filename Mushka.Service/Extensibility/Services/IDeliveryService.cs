using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface IDeliveryService
    {
        Task<ValidationResponse<Delivery>> AddAsync(Delivery delivery, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<Delivery>>> GetAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}