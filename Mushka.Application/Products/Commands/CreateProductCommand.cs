using Mediator;

namespace Mushka.Application.Products.Commands;

public sealed record CreateProductCommand(ProductData Product) : ICommand;
