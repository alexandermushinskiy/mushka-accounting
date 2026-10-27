using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Supplies.Commands;

public sealed record CreateSupplyCommand(
    SupplyCommandModel Supply,
    IReadOnlyList<SupplyProductCommandModel> Products) : ICommand;
