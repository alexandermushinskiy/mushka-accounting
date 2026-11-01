using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Orders.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Models;

namespace Mushka.Application.Orders.Queries.Search;

public sealed class SearchOrdersQueryHandler : IQueryHandler<SearchOrdersQuery, SearchOrdersReadModel>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISearchOrdersConverter _converter;

    public SearchOrdersQueryHandler(IStorage storage, ISearchOrdersConverter converter)
    {
        _orderRepository = storage.GetRepository<IOrderRepository>();
        _converter = converter;
    }

    public async ValueTask<SearchOrdersReadModel> Handle(SearchOrdersQuery query, CancellationToken cancellationToken)
    {
        var filter = new SearchOrdersFilter
        {
            CustomerName = query.CustomerName,
            OrderDate = new DateRange(query.OrderDateFrom, query.OrderDateTo),
            CurrentPage = query.CurrentPage,
            PageSize = query.PageSize,
            SortKey = query.SortKey,
            SortOrder = query.SortOrder
        };

        var orders = await _orderRepository.SearchAsync(filter, cancellationToken);
        var total = await _orderRepository.GetCountAsync(filter, cancellationToken);

        return _converter.ToReadModel(orders, total);
    }
}
