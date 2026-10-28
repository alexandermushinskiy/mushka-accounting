using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Products.Queries.GetByCategory;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public sealed class GetProductsByCategoryConverter : IGetProductsByCategoryConverter
{
    public GetProductsByCategoryReadModel ToReadModel(IEnumerable<Product> products) =>
        new GetProductsByCategoryReadModel
        {
            Items = products.Select(ToItem).ToList()
        };

    private static ProductByCategoryReadModel ToItem(Product product)
    {
        var lastSupply = product.Supplies
            .OrderByDescending(delivery => delivery.Supply.ReceivedDate)
            .Select(delivery => delivery.Supply)
            .FirstOrDefault();

        var lastSupplyCount = lastSupply?.Products
            .Where(delivery => delivery.ProductId == product.Id)
            .Select(delivery => delivery.Quantity)
            .Single();

        return new ProductByCategoryReadModel
        {
            Id = product.Id,
            Name = product.Name,
            VendorCode = product.VendorCode,
            RecommendedPrice = product.RecommendedPrice,
            CreatedOn = product.CreatedOn,
            Quantity = product.Quantity,
            DeliveriesCount = product.Supplies.Count,
            LastDeliveryDate = lastSupply?.ReceivedDate,
            LastDeliveryCount = lastSupplyCount,
            SizeName = product.Size?.Name
        };
    }
}
