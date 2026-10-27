using System.Collections.Generic;
using Mushka.Application.Supplies.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Converters
{
    public interface ISearchSuppliesConverter
    {
        SearchSuppliesReadModel ToReadModel(IEnumerable<Supply> supplies, int totalCount);
    }
}
