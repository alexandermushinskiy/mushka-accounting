using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Domain.Dto;
using Mushka.Application.Analytics.Providers;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Analytics.Queries.GetOrdersCount;

public sealed class GetOrdersCountQueryHandler : IQueryHandler<GetOrdersCountQuery, OrdersCountReadModel>
{
    private readonly IAnalyticsRepository _analyticsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAnalyticsPeriodProvider _analyticsPeriodProvider;

    public GetOrdersCountQueryHandler(
        IStorage storage,
        IDateTimeProvider dateTimeProvider,
        IAnalyticsPeriodProvider analyticsPeriodProvider)
    {
        _analyticsRepository = storage.GetRepository<IAnalyticsRepository>();
        _dateTimeProvider = dateTimeProvider;
        _analyticsPeriodProvider = analyticsPeriodProvider;
    }

    public async ValueTask<OrdersCountReadModel> Handle(GetOrdersCountQuery query, CancellationToken cancellationToken)
    {
        var limitDate = _analyticsPeriodProvider.StartOfWindow(_dateTimeProvider.GetNow(), query.Period);
        var orders = await _analyticsRepository.GetOrdersCount(limitDate, cancellationToken);

        var readModel = ToReadModel(orders);
        return readModel;
    }

    private static OrdersCountReadModel ToReadModel(IEnumerable<OrdersCount> orders) =>
        new OrdersCountReadModel
        {
            Data = orders.Select(order => new OrdersCountItemReadModel
            {
                CreatedOn = order.CreatedOn,
                Quantity = order.Quantity
            }).ToList()
        };
}
