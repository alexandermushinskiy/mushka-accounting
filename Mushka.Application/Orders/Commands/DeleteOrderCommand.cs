using System;
using Mediator;

namespace Mushka.Application.Orders.Commands;

public sealed record DeleteOrderCommand(Guid Id) : ICommand;
