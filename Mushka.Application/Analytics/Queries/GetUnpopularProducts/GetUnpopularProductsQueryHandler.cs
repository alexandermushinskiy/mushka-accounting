using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Analytics.Converters;
using Mushka.Application.Analytics.Queries.GetPopularProducts;
using Mushka.Domain.Dto;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Analytics.Queries.GetUnpopularProducts;

public sealed class GetUnpopularProductsQueryHandler : IQueryHandler<GetUnpopularProductsQuery, PopularProductsReadModel>
{
    private const int DefaultTopCount = 7;

    private readonly IAnalyticsRepository _analyticsRepository;
    private readonly IAnalyticConverter _analyticConverter;

    public GetUnpopularProductsQueryHandler(IStorage storage, IAnalyticConverter analyticConverter)
    {
        _analyticsRepository = storage.GetRepository<IAnalyticsRepository>();
        _analyticConverter = analyticConverter;
    }

    public async ValueTask<PopularProductsReadModel> Handle(GetUnpopularProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _analyticsRepository.GetProductsByPopularity(
            DefaultTopCount,
            Popularity.Unpopular,
            cancellationToken);

        var readModel = ToReadModel(products);
        return readModel;
    }

    private PopularProductsReadModel ToReadModel(IEnumerable<PopularProduct> products) =>
        _analyticConverter.ToPopularProducts(products);
}
