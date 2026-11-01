using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Orders.Commands;

public sealed record UpdateOrderCommand(
    Guid Id,
    OrderData Order,
    OrderCustomerData Customer,
    IReadOnlyList<OrderProductData> Products) : ICommand;
