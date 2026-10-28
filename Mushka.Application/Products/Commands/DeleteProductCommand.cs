using System;
using Mediator;

namespace Mushka.Application.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : ICommand;
