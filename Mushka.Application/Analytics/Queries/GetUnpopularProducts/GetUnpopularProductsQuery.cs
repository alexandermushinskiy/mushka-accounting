using Mediator;
using Mushka.Application.Analytics.Queries.GetPopularProducts;

namespace Mushka.Application.Analytics.Queries.GetUnpopularProducts;

public sealed record GetUnpopularProductsQuery : IQuery<PopularProductsReadModel>;
