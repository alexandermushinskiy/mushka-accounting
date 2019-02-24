using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;

namespace Mushka.Service.Extensibility.Services
{
    public interface IAnalyticsService
    {
        Task<ValidationResponse<IEnumerable<PopularProduct>>> GetPopularProducts(CancellationToken cancellationToken = default(CancellationToken));
    }
}