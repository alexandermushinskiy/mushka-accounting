using System;
using Mediator;

namespace Mushka.Application.Products.Queries.GetCostPrice;

public sealed record GetProductCostPriceQuery(Guid Id, int ProductsCount) : IQuery<GetProductCostPriceReadModel>;
