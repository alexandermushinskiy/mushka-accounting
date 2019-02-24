using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class AnalyticsService : ServiceBase, IAnalyticsService
    {
        private const int DefaultTopCount = 7;
        private readonly IAnalyticsRepository analyticsRepository;

        public AnalyticsService(
            IStorage storage,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            analyticsRepository = storage.GetRepository<IAnalyticsRepository>();
        }

        public async Task<ValidationResponse<IEnumerable<PopularProduct>>> GetPopularProducts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var populars = await analyticsRepository.GetPopularProducts(DefaultTopCount, cancellationToken);

            return CreateInfoValidationResponse(populars, "Popular products were retrived successfully.");
        }
    }
}