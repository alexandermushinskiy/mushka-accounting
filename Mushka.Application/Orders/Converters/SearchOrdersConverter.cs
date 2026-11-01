using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Orders.Queries.Search;
using Mushka.Domain.Dto;

namespace Mushka.Application.Orders.Converters;

public sealed class SearchOrdersConverter : ISearchOrdersConverter
{
    public SearchOrdersReadModel ToReadModel(IEnumerable<OrderSummaryDto> orders, int total)
    {
        var items = (orders ?? Enumerable.Empty<OrderSummaryDto>()).Select(ToItem).ToList();

        return new SearchOrdersReadModel
        {
            Total = total,
            Items = items
        };
    }

    private static SearchOrderReadModel ToItem(OrderSummaryDto order) =>
        new SearchOrderReadModel
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            OrderNumber = order.OrderNumber,
            Cost = order.Cost,
            Address = order.Address,
            CustomerName = order.CustomerName,
            ProductsCount = order.ProductsCount,
            IsWholesale = order.IsWholesale
        };
}
