using System.Collections.Generic;
using Mushka.Application.CorporateOrders.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.CorporateOrders.Converters;

public interface ISearchCorporateOrdersConverter
{
    SearchCorporateOrdersReadModel ToReadModel(IEnumerable<CorporateOrder> corporateOrders);
}
