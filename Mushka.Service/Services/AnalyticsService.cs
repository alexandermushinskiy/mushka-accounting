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
            var popularProducts = await analyticsRepository.GetPopularProducts(DefaultTopCount, cancellationToken);

            return CreateInfoValidationResponse(popularProducts, "Popular products were retrived successfully.");
        }

        public async Task<ValidationResponse<IEnumerable<PopularCity>>> GetPopularCities(CancellationToken cancellationToken = default(CancellationToken))
        {
            var popularCities = await analyticsRepository.GetPopularCities(DefaultTopCount, cancellationToken);

            return CreateInfoValidationResponse(popularCities, "Popular cities were retrived successfully.");
        }

        public async Task<ValidationResponse<Balance>> GetBalance(CancellationToken cancellationToken = default(CancellationToken))
        {
            var balance = await analyticsRepository.GetBalance(cancellationToken);

            return CreateInfoValidationResponse(balance, "Balance was retrived successfully.");
        }
    }
}