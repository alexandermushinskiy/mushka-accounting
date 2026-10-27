using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Supplies.Queries.Export
{
    public sealed record ExportSuppliesQuery(
        IEnumerable<Guid> SupplyIds,
        IEnumerable<Guid> ProductIds) : IQuery<ExportSuppliesReadModel>;
}
