using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Orders.Queries.GetDefaultProducts;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Converters;

public sealed class GetDefaultOrderProductsConverter : IGetDefaultOrderProductsConverter
{
    public GetDefaultOrderProductsReadModel ToReadModel(IEnumerable<OrderProduct> products) =>
        new GetDefaultOrderProductsReadModel
        {
            Items = products.Select(ToProduct).ToList()
        };

    private static GetDefaultOrderProductReadModel ToProduct(OrderProduct orderProduct) =>
        new GetDefaultOrderProductReadModel
        {
            Id = orderProduct.ProductId,
            Name = orderProduct.Product?.Name,
            VendorCode = orderProduct.Product?.VendorCode,
            SizeName = orderProduct.Product?.Size?.Name,
            Quantity = orderProduct.Quantity,
            UnitPrice = orderProduct.UnitPrice,
            CostPrice = orderProduct.CostPrice
        };
}
