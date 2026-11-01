using Mediator;

namespace Mushka.Application.Analytics.Queries.GetSoldProductsCount;

public sealed record GetSoldProductsCountQuery(int Period) : IQuery<SoldProductsCountReadModel>;
