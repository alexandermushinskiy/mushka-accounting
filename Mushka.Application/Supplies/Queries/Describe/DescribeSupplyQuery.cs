using System;
using Mediator;

namespace Mushka.Application.Supplies.Queries.Describe;

public sealed record DescribeSupplyQuery(Guid Id) : IQuery<DescribeSupplyReadModel>;
