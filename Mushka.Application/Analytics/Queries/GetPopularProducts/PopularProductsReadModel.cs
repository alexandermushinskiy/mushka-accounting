using System.Collections.Generic;

namespace Mushka.Application.Analytics.Queries.GetPopularProducts;

public sealed class PopularProductsReadModel
{
    public IEnumerable<PopularProductReadModel> Data { get; set; }
}

public sealed class PopularProductReadModel
{
    public string Name { get; set; }

    public string SizeName { get; set; }

    public string VendorCode { get; set; }

    public int Quantity { get; set; }
}
