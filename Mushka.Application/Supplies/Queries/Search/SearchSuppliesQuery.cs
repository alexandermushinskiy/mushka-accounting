using System;
using Mediator;

namespace Mushka.Application.Supplies.Queries.Search
{
    public sealed record SearchSuppliesQuery(string SearchKey, Guid? ProductId) : IQuery<SearchSuppliesReadModel>;
}
