using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.CorporateOrders.Commands;

public sealed record CreateCorporateOrderCommand(
    CorporateOrderData Order,
    IReadOnlyList<CorporateOrderProductData> Products) : ICommand;
