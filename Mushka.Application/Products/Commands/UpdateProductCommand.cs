using System;
using Mediator;

namespace Mushka.Application.Products.Commands;

public sealed record UpdateProductCommand(Guid Id, ProductData Product) : ICommand;
