using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface IExhibitionService : IServiceBase<Exhibition>
    {
        Task<ValidationResponse<IEnumerable<ExhibitionProduct>>> GetDefaultProducts(CancellationToken cancellationToken = default(CancellationToken));
    }
}