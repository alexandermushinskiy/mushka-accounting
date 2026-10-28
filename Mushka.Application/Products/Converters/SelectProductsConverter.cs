using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Products.Queries.Select;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public sealed class SelectProductsConverter : ISelectProductsConverter
{
    public SelectProductsReadModel ToReadModel(IEnumerable<Product> products) =>
        new SelectProductsReadModel
        {
            Items = products.Select(ToItem).ToList()
        };

    private static SelectProductReadModel ToItem(Product product) =>
        new SelectProductReadModel
        {
            Id = product.Id,
            Name = product.Name,
            VendorCode = product.VendorCode,
            RecommendedPrice = product.RecommendedPrice,
            Quantity = product.Quantity,
            CategoryName = product.Category.Name,
            SizeName = product.Size?.Name,
            IsArchival = product.IsArchival
        };
}
