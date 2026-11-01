using System.Collections.Generic;
using Mushka.Application.Orders.Queries.GetDefaultProducts;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Converters;

public interface IGetDefaultOrderProductsConverter
{
    GetDefaultOrderProductsReadModel ToReadModel(IEnumerable<OrderProduct> products);
}
