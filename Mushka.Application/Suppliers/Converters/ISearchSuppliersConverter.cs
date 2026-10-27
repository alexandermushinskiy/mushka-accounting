using System.Collections.Generic;
using Mushka.Application.Suppliers.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Suppliers.Converters
{
    public interface ISearchSuppliersConverter
    {
        SearchSuppliersReadModel ToReadModel(IEnumerable<Supplier> suppliers);
    }
}
