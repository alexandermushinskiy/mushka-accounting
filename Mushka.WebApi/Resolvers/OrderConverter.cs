using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;

namespace Mushka.WebApi.Resolvers
{
    public class OrderConverter : ITypeConverter<Order, OrderModel>
    {
        public OrderModel Convert(Order source, OrderModel destination, ResolutionContext context) =>
            new OrderModel
            {
                Id = source.Id,
                OrderDate = source.OrderDate,
                PaymentType = source.PaymentType,
                Products = source.Products.Select(CreateOrderProductModel)
            };

        private static OrderProductModel CreateOrderProductModel(OrderProduct orderProduct) =>
            new OrderProductModel
            {
                ProductId = orderProduct.ProductId,
                ProductName = orderProduct.Product?.Name,
                SizeId = orderProduct.SizeId,
                SizeName = orderProduct.Size?.Name,
                Quantity = orderProduct.Quantity,
                PriceForItem = orderProduct.PriceForItem
            };
    }
}