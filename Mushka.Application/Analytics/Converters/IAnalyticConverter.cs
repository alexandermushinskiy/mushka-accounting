using System.Collections.Generic;
using Mushka.Application.Analytics.Queries.GetPopularProducts;
using Mushka.Domain.Dto;

namespace Mushka.Application.Analytics.Converters;

public interface IAnalyticConverter
{
    PopularProductsReadModel ToPopularProducts(IEnumerable<PopularProduct> products);
}
