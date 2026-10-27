using System;
using System.Collections.Generic;

namespace Mushka.Application.Exhibitions.Queries.GetDefaultProducts;

public sealed class GetDefaultExhibitionProductsReadModel
{
    public IEnumerable<GetDefaultExhibitionProductReadModel> Products { get; set; }
}

public sealed class GetDefaultExhibitionProductReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VendorCode { get; set; }

    public string SizeName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal CostPrice { get; set; }
}
