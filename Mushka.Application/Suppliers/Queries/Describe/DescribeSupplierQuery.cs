using System;
using Mediator;

namespace Mushka.Application.Suppliers.Queries.Describe;

public sealed record DescribeSupplierQuery(Guid Id) : IQuery<DescribeSupplierReadModel>;
