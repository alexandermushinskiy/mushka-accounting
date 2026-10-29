using System;
using Mediator;

namespace Mushka.Application.CorporateOrders.Commands;

public sealed record DeleteCorporateOrderCommand(Guid Id) : ICommand;
