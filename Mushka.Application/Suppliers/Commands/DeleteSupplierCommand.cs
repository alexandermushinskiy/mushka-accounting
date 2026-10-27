using System;
using Mediator;

namespace Mushka.Application.Suppliers.Commands;

public sealed record DeleteSupplierCommand(Guid Id) : ICommand;
