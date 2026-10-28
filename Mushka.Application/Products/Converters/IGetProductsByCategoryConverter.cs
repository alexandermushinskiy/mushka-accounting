using System.Collections.Generic;
using Mushka.Application.Products.Queries.GetByCategory;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public interface IGetProductsByCategoryConverter
{
    GetProductsByCategoryReadModel ToReadModel(IEnumerable<Product> products);
}
