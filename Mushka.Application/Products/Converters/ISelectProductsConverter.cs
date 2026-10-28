using System.Collections.Generic;
using Mushka.Application.Products.Queries.Select;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public interface ISelectProductsConverter
{
    SelectProductsReadModel ToReadModel(IEnumerable<Product> products);
}
