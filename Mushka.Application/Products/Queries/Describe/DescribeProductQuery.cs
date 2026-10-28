using System;
using Mediator;

namespace Mushka.Application.Products.Queries.Describe;

public sealed record DescribeProductQuery(Guid Id) : IQuery<DescribeProductReadModel>;
