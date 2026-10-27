using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Exhibitions.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public sealed class SearchExhibitionsConverter : ISearchExhibitionsConverter
{
    public SearchExhibitionsReadModel ToReadModel(IEnumerable<Exhibition> exhibitions)
    {
        var items = exhibitions.Select(ToItem).ToList();

        return new SearchExhibitionsReadModel
        {
            Total = items.Count,
            Items = items
        };
    }

    private static SearchExhibitionReadModel ToItem(Exhibition exhibition) =>
        new SearchExhibitionReadModel
        {
            Id = exhibition.Id,
            Name = exhibition.Name,
            FromDate = exhibition.FromDate,
            ToDate = exhibition.ToDate,
            City = exhibition.City,
            ParticipationCost = exhibition.ParticipationCost,
            ProductsCount = exhibition.Products?.Sum(product => product.Quantity) ?? 0
        };
}
