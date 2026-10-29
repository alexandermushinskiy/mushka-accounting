using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.CorporateOrders.Queries.Search;

public sealed class SearchCorporateOrdersQueryHandler : IQueryHandler<SearchCorporateOrdersQuery, SearchCorporateOrdersReadModel>
{
    private readonly ICorporateOrderRepository _corporateOrderRepository;
    private readonly ISearchCorporateOrdersConverter _converter;

    public SearchCorporateOrdersQueryHandler(IStorage storage, ISearchCorporateOrdersConverter converter)
    {
        _corporateOrderRepository = storage.GetRepository<ICorporateOrderRepository>();
        _converter = converter;
    }

    public async ValueTask<SearchCorporateOrdersReadModel> Handle(SearchCorporateOrdersQuery query, CancellationToken cancellationToken)
    {
        var corporateOrders = (await _corporateOrderRepository.GetAllAsync(cancellationToken))
            .OrderBy(order => order.CreatedOn)
            .ToList();

        return _converter.ToReadModel(corporateOrders);
    }
}
