using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Products.Queries.GetSizes;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public sealed class GetProductSizesConverter : IGetProductSizesConverter
{
    public GetProductSizesReadModel ToReadModel(IEnumerable<Size> sizes) =>
        new GetProductSizesReadModel
        {
            Items = sizes.Select(size => new ProductSizeReadModel
            {
                Id = size.Id,
                Name = size.Name
            }).ToList()
        };
}
