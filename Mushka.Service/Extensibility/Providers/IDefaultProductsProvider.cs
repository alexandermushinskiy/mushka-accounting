using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Providers
{
    public interface IDefaultProductsProvider
    {
        Task<ValidationResponse<IEnumerable<OrderProduct>>> GetOrderDefaultProducts(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<ExhibitionProduct>>> GetExhibitionProducts(CancellationToken cancellationToken = default(CancellationToken));
    }
}