using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Supplies.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Models;

namespace Mushka.Application.Supplies.Queries.Search;

public sealed class SearchSuppliesQueryHandler : IQueryHandler<SearchSuppliesQuery, SearchSuppliesReadModel>
{
    private readonly ISupplyRepository _supplyRepository;
    private readonly ISearchSuppliesConverter _converter;

    public SearchSuppliesQueryHandler(IStorage storage, ISearchSuppliesConverter converter)
    {
        _supplyRepository = storage.GetRepository<ISupplyRepository>();
        _converter = converter;
    }

    public async ValueTask<SearchSuppliesReadModel> Handle(SearchSuppliesQuery query, CancellationToken cancellationToken)
    {
        var totalCount = await _supplyRepository.GetAllCountAsync(cancellationToken);
        var supplies = await _supplyRepository.GetByFilterAsync(
            new SearchSuppliesFilter
            {
                SearchKey = query.SearchKey,
                ProductId = query.ProductId
            },
            cancellationToken);

        return _converter.ToReadModel(supplies, totalCount);
    }
}
