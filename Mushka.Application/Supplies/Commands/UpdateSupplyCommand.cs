using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Supplies.Commands;

public sealed record UpdateSupplyCommand(
    Guid Id,
    SupplyData Supply,
    IReadOnlyList<SupplyProductData> Products) : ICommand;
