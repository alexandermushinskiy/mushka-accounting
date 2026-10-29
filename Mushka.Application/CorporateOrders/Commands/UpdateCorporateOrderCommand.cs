using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.CorporateOrders.Commands;

public sealed record UpdateCorporateOrderCommand(
    Guid Id,
    CorporateOrderData Order,
    IReadOnlyList<CorporateOrderProductData> Products) : ICommand;
