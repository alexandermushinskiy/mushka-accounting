using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Supplies.Commands
{
    public sealed record UpdateSupplyCommand(
        Guid Id,
        SupplyCommandModel Supply,
        IReadOnlyList<SupplyProductCommandModel> Products) : ICommand;
}
