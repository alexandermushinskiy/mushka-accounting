using System;
using System.Collections.Generic;

namespace Mushka.Application.Analytics.Queries.GetSoldProductsCount;

public sealed class SoldProductsCountReadModel
{
    public IEnumerable<SoldProductsCountItemReadModel> Data { get; set; }
}

public sealed class SoldProductsCountItemReadModel
{
    public DateTime CreatedOn { get; set; }

    public int Quantity { get; set; }
}
