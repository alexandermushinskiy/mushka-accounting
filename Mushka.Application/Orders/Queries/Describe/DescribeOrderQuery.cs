using System;
using Mediator;

namespace Mushka.Application.Orders.Queries.Describe;

public sealed record DescribeOrderQuery(Guid Id) : IQuery<DescribeOrderReadModel>;
