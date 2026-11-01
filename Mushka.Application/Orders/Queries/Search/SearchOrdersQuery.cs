using System;
using Mediator;

namespace Mushka.Application.Orders.Queries.Search;

public sealed record SearchOrdersQuery(
    string CustomerName,
    DateTime? OrderDateFrom,
    DateTime? OrderDateTo,
    int CurrentPage,
    int PageSize,
    string SortKey,
    string SortOrder) : IQuery<SearchOrdersReadModel>;
