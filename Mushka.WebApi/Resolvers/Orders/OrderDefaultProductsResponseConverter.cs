using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order.GetDefaultProducts;

namespace Mushka.WebApi.Resolvers.Orders
{
    public class OrderDefaultProductsResponseConverter :
         ITypeConverter<OperationResult<IEnumerable<OrderProduct>>, OrderDefaultProductResponseModel>
    {
        public OrderDefaultProductResponseModel Convert(
            OperationResult<IEnumerable<OrderProduct>> source,
            OrderDefaultProductResponseModel destination,
            ResolutionContext context)
        {
            return new OrderDefaultProductResponseModel
            {
                Items = source.Data.Select(orderProduct => new OrderDefaultProductModel
                {
                    Id = orderProduct.ProductId,
                    Name = orderProduct.Product?.Name,
                    VendorCode = orderProduct.Product?.VendorCode,
                    SizeName = orderProduct.Product?.Size?.Name,
                    Quantity = orderProduct.Quantity,
                    UnitPrice = orderProduct.UnitPrice,
                    CostPrice = orderProduct.CostPrice
                })
            };
        }
    }
}