using System;
using System.Collections.Generic;

namespace Mushka.Application.Products.Queries.GetSizes;

public sealed class GetProductSizesReadModel
{
    public IEnumerable<ProductSizeReadModel> Items { get; set; }
}

public sealed class ProductSizeReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
