using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Supplies.Commands;

public sealed record CreateSupplyCommand(
    SupplyData Supply,
    IReadOnlyList<SupplyProductData> Products) : ICommand;
