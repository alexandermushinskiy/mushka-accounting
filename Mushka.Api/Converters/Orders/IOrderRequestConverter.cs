using System;
using Mushka.Api.ClientModels.Order;
using Mushka.Application.Orders.Commands;
using Mushka.Application.Orders.Queries.Search;

namespace Mushka.Api.Converters.Orders;

public interface IOrderRequestConverter
{
    SearchOrdersQuery ToSearchQuery(SearchOrdersRequestModel request);

    CreateOrderCommand ToCreateCommand(OrderRequestModel request);

    UpdateOrderCommand ToUpdateCommand(Guid id, OrderRequestModel request);
}
