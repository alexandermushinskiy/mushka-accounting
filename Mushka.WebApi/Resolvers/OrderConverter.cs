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
                Number = source.Number,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                FirstName = source.Customer.FirstName,
                LastName = source.Customer.LastName,
                Phone = source.Customer.Phone,
                Email = source.Customer.Email,
                Region = source.Customer.Region,
                City = source.Customer.City,
                Products = source.Products.Select(CreateOrderProductModel)
            };

        private static OrderProductModel CreateOrderProductModel(OrderProduct orderProduct) =>
            new OrderProductModel
            {
                Id = orderProduct.ProductId,
                Name = orderProduct.Product?.Name,
                VendorCode = orderProduct.Product?.VendorCode,
                Size = orderProduct.Product?.Size?.Name,
                Quantity = orderProduct.Quantity,
                UnitPrice = orderProduct.UnitPrice,
            };
    }
}