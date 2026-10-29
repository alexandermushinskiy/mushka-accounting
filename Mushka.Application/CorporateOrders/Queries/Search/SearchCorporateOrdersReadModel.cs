using System;
using System.Collections.Generic;

namespace Mushka.Application.CorporateOrders.Queries.Search;

public sealed class SearchCorporateOrdersReadModel
{
    public int Total { get; set; }

    public IEnumerable<SearchCorporateOrderReadModel> Items { get; set; }
}

public sealed class SearchCorporateOrderReadModel
{
    public Guid Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public string OrderNumber { get; set; }

    public string CompanyName { get; set; }

    public string Address { get; set; }
}
