using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Orders.Commands;

public sealed record CreateOrderCommand(
    OrderData Order,
    OrderCustomerData Customer,
    IReadOnlyList<OrderProductData> Products) : ICommand;
