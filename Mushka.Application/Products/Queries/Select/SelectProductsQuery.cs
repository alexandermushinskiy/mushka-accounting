using Mediator;

namespace Mushka.Application.Products.Queries.Select;

public sealed record SelectProductsQuery(bool InStock) : IQuery<SelectProductsReadModel>;
