using System.Collections.Generic;

namespace Mushka.Application.Analytics.Queries.GetPopularCities;

public sealed class PopularCitiesReadModel
{
    public IEnumerable<PopularCityReadModel> Data { get; set; }
}

public sealed class PopularCityReadModel
{
    public string City { get; set; }

    public int Quantity { get; set; }
}
