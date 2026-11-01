using System;
using System.Collections.Generic;

namespace Mushka.Application.Orders.Queries.GetDefaultProducts;

public sealed class GetDefaultOrderProductsReadModel
{
    public IEnumerable<GetDefaultOrderProductReadModel> Items { get; set; }
}

public sealed class GetDefaultOrderProductReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VendorCode { get; set; }

    public string SizeName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal CostPrice { get; set; }
}
