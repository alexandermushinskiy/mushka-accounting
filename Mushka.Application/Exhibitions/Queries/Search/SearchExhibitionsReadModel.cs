using System;
using System.Collections.Generic;

namespace Mushka.Application.Exhibitions.Queries.Search;

public sealed class SearchExhibitionsReadModel
{
    public int Total { get; set; }

    public IEnumerable<SearchExhibitionReadModel> Items { get; set; }
}

public sealed class SearchExhibitionReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string City { get; set; }

    public decimal ParticipationCost { get; set; }

    public int ProductsCount { get; set; }
}
