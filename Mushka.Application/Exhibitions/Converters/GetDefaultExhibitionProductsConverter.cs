using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Exhibitions.Queries.GetDefaultProducts;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public sealed class GetDefaultExhibitionProductsConverter : IGetDefaultExhibitionProductsConverter
{
    public GetDefaultExhibitionProductsReadModel ToReadModel(IEnumerable<ExhibitionProduct> products) =>
        new GetDefaultExhibitionProductsReadModel
        {
            Products = products.Select(ToProduct).ToList()
        };

    private static GetDefaultExhibitionProductReadModel ToProduct(ExhibitionProduct exhibitionProduct) =>
        new GetDefaultExhibitionProductReadModel
        {
            Id = exhibitionProduct.ProductId,
            Name = exhibitionProduct.Product?.Name,
            VendorCode = exhibitionProduct.Product?.VendorCode,
            SizeName = exhibitionProduct.Product?.Size?.Name,
            Quantity = exhibitionProduct.Quantity,
            UnitPrice = exhibitionProduct.UnitPrice,
            CostPrice = exhibitionProduct.CostPrice
        };
}
