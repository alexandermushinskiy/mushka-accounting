using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Orders.Queries.Export;

public sealed record ExportOrdersQuery(string Title, IEnumerable<Guid> OrderIds) : IQuery<ExportOrdersReadModel>;
