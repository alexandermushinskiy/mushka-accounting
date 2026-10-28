using System;
using Mediator;

namespace Mushka.Application.Categories.Queries.Describe;

public sealed record DescribeCategoryQuery(Guid Id) : IQuery<DescribeCategoryReadModel>;
