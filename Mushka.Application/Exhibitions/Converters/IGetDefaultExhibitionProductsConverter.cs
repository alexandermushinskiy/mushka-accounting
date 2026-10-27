using System.Collections.Generic;
using Mushka.Application.Exhibitions.Queries.GetDefaultProducts;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public interface IGetDefaultExhibitionProductsConverter
{
    GetDefaultExhibitionProductsReadModel ToReadModel(IEnumerable<ExhibitionProduct> products);
}
