using System.Collections.Generic;
using Mushka.Application.Orders.Queries.Search;
using Mushka.Domain.Dto;

namespace Mushka.Application.Orders.Converters;

public interface ISearchOrdersConverter
{
    SearchOrdersReadModel ToReadModel(IEnumerable<OrderSummaryDto> orders, int total);
}
