using System;
using System.Collections.Generic;

namespace Mushka.Application.Products.Queries.Select;

public sealed class SelectProductsReadModel
{
    public IEnumerable<SelectProductReadModel> Items { get; set; }
}

public sealed class SelectProductReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VendorCode { get; set; }

    public decimal? RecommendedPrice { get; set; }

    public int Quantity { get; set; }

    public string CategoryName { get; set; }

    public string SizeName { get; set; }

    public bool IsArchival { get; set; }
}
