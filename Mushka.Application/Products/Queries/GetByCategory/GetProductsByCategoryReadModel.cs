using System;
using System.Collections.Generic;

namespace Mushka.Application.Products.Queries.GetByCategory;

public sealed class GetProductsByCategoryReadModel
{
    public IEnumerable<ProductByCategoryReadModel> Items { get; set; }
}

public sealed class ProductByCategoryReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VendorCode { get; set; }

    public decimal? RecommendedPrice { get; set; }

    public DateTime CreatedOn { get; set; }

    public int Quantity { get; set; }

    public int? DeliveriesCount { get; set; }

    public DateTime? LastDeliveryDate { get; set; }

    public int? LastDeliveryCount { get; set; }

    public string SizeName { get; set; }
}
