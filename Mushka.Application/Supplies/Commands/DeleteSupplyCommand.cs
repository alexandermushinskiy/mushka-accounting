using System;
using Mediator;

namespace Mushka.Application.Supplies.Commands
{
    public sealed record DeleteSupplyCommand(Guid Id) : ICommand;
}
