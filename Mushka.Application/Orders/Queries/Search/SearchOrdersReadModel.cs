using System;
using System.Collections.Generic;

namespace Mushka.Application.Orders.Queries.Search;

public sealed class SearchOrdersReadModel
{
    public int Total { get; set; }

    public IEnumerable<SearchOrderReadModel> Items { get; set; }
}

public sealed class SearchOrderReadModel
{
    public Guid Id { get; set; }

    public DateTime OrderDate { get; set; }

    public string OrderNumber { get; set; }

    public decimal Cost { get; set; }

    public string Address { get; set; }

    public string CustomerName { get; set; }

    public int ProductsCount { get; set; }

    public bool IsWholesale { get; set; }
}
