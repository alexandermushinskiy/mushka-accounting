using System;
using Mediator;

namespace Mushka.Application.Exhibitions.Queries.Describe;

public sealed record DescribeExhibitionQuery(Guid Id) : IQuery<DescribeExhibitionReadModel>;
