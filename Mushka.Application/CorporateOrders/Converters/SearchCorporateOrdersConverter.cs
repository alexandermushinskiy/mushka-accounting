using System.Collections.Generic;
using System.Linq;
using Mushka.Application.CorporateOrders.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.CorporateOrders.Converters;

public sealed class SearchCorporateOrdersConverter : ISearchCorporateOrdersConverter
{
    public SearchCorporateOrdersReadModel ToReadModel(IEnumerable<CorporateOrder> corporateOrders)
    {
        var items = corporateOrders.Select(ToItem).ToList();

        return new SearchCorporateOrdersReadModel
        {
            Total = items.Count,
            Items = items
        };
    }

    private static SearchCorporateOrderReadModel ToItem(CorporateOrder corporateOrder) =>
        new SearchCorporateOrderReadModel
        {
            Id = corporateOrder.Id,
            CreatedOn = corporateOrder.CreatedOn,
            OrderNumber = corporateOrder.Number,
            CompanyName = corporateOrder.CompanyName,
            Address = $"{corporateOrder.Region}, {corporateOrder.City}"
        };
}
