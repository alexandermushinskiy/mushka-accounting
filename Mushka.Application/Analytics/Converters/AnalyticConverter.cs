using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Analytics.Queries.GetPopularProducts;
using Mushka.Domain.Dto;

namespace Mushka.Application.Analytics.Converters;

public sealed class AnalyticConverter : IAnalyticConverter
{
    public PopularProductsReadModel ToPopularProducts(IEnumerable<PopularProduct> products) =>
        new PopularProductsReadModel
        {
            Data = products.Select(product => new PopularProductReadModel
            {
                Name = product.Name,
                SizeName = product.SizeName,
                VendorCode = product.VendorCode,
                Quantity = product.Quantity
            }).ToList()
        };
}
