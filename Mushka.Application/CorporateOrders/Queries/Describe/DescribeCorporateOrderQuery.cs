using System;
using Mediator;

namespace Mushka.Application.CorporateOrders.Queries.Describe;

public sealed record DescribeCorporateOrderQuery(Guid Id) : IQuery<DescribeCorporateOrderReadModel>;
