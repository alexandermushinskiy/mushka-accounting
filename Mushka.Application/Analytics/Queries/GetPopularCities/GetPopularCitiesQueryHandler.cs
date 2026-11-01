using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Domain.Dto;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Analytics.Queries.GetPopularCities;

public sealed class GetPopularCitiesQueryHandler : IQueryHandler<GetPopularCitiesQuery, PopularCitiesReadModel>
{
    private const int DefaultTopCount = 7;

    private readonly IAnalyticsRepository _analyticsRepository;

    public GetPopularCitiesQueryHandler(IStorage storage)
    {
        _analyticsRepository = storage.GetRepository<IAnalyticsRepository>();
    }

    public async ValueTask<PopularCitiesReadModel> Handle(GetPopularCitiesQuery query, CancellationToken cancellationToken)
    {
        var cities = await _analyticsRepository.GetPopularCities(DefaultTopCount, cancellationToken);

        var readModel = ToReadModel(cities);
        return readModel;
    }

    private static PopularCitiesReadModel ToReadModel(IEnumerable<PopularCity> cities) =>
        new PopularCitiesReadModel
        {
            Data = cities.Select(city => new PopularCityReadModel
            {
                City = city.City,
                Quantity = city.Quantity
            }).ToList()
        };
}
