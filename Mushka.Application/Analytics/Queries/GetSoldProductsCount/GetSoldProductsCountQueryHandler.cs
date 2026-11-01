using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Domain.Dto;
using Mushka.Application.Analytics.Providers;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Analytics.Queries.GetSoldProductsCount;

public sealed class GetSoldProductsCountQueryHandler : IQueryHandler<GetSoldProductsCountQuery, SoldProductsCountReadModel>
{
    private readonly IAnalyticsRepository _analyticsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAnalyticsPeriodProvider _analyticsPeriodProvider;

    public GetSoldProductsCountQueryHandler(
        IStorage storage,
        IDateTimeProvider dateTimeProvider,
        IAnalyticsPeriodProvider analyticsPeriodProvider)
    {
        _analyticsRepository = storage.GetRepository<IAnalyticsRepository>();
        _dateTimeProvider = dateTimeProvider;
        _analyticsPeriodProvider = analyticsPeriodProvider;
    }

    public async ValueTask<SoldProductsCountReadModel> Handle(GetSoldProductsCountQuery query, CancellationToken cancellationToken)
    {
        var limitDate = _analyticsPeriodProvider.StartOfWindow(_dateTimeProvider.GetNow(), query.Period);
        var soldProducts = await _analyticsRepository.GetSoldProductsCount(limitDate, cancellationToken);

        var readModel = ToReadModel(soldProducts);
        return readModel;
    }

    private static SoldProductsCountReadModel ToReadModel(IEnumerable<SoldProductsCount> soldProducts) =>
        new SoldProductsCountReadModel
        {
            Data = soldProducts.Select(item => new SoldProductsCountItemReadModel
            {
                CreatedOn = item.CreatedOn,
                Quantity = item.Quantity
            }).ToList()
        };
}
