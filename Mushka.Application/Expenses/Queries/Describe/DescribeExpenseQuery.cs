using System;
using Mediator;

namespace Mushka.Application.Expenses.Queries.Describe;

public sealed record DescribeExpenseQuery(Guid Id) : IQuery<DescribeExpenseReadModel>;
