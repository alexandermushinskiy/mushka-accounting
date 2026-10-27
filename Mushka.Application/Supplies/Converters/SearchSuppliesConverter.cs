using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Supplies.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Converters;

public sealed class SearchSuppliesConverter : ISearchSuppliesConverter
{
    public SearchSuppliesReadModel ToReadModel(IEnumerable<Supply> supplies, int totalCount) =>
        new SearchSuppliesReadModel
        {
            Total = totalCount,
            Items = supplies.Select(ToItem).ToList()
        };

    private static SearchSupplyReadModel ToItem(Supply supply) =>
        new SearchSupplyReadModel
        {
            Id = supply.Id,
            Description = supply.Description,
            SupplierName = supply.Supplier?.Name,
            ReceivedDate = supply.ReceivedDate,
            Cost = supply.Cost,
            TotalCost = supply.TotalCost,
            ProductsAmount = supply.Products?.Sum(product => product.Quantity) ?? 0
        };
}
