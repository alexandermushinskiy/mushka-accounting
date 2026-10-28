using System.Collections.Generic;
using Mushka.Application.Products.Queries.GetSizes;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public interface IGetProductSizesConverter
{
    GetProductSizesReadModel ToReadModel(IEnumerable<Size> sizes);
}
