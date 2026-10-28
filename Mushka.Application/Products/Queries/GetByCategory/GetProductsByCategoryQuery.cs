using System;
using Mediator;

namespace Mushka.Application.Products.Queries.GetByCategory;

public sealed record GetProductsByCategoryQuery(Guid CategoryId) : IQuery<GetProductsByCategoryReadModel>;
