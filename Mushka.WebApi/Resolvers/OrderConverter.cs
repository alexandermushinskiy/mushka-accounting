using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;

namespace Mushka.WebApi.Resolvers
{
    public class OrderConverter : ITypeConverter<Order, OrderModel>, ITypeConverter<OrderProduct, OrderProductModel>
    {
        public OrderModel Convert(Order source, OrderModel destination, ResolutionContext context) =>
            new OrderModel
            {
                Id = source.Id,
                OrderDate = source.OrderDate,
                Number = source.Number,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                Discount = source.Discount ?? 0,
                Notes = source.Notes,
                FirstName = source.Customer.FirstName,
                LastName = source.Customer.LastName,
                Phone = source.Customer.Phone,
                Email = source.Customer.Email,
                Region = source.Customer.Region,
                City = source.Customer.City,
                Products = source.Products.Select(CreateOrderProductModel)
            };

        public OrderProductModel Convert(OrderProduct source, OrderProductModel destination, ResolutionContext context) =>
            CreateOrderProductModel(source);

        private static OrderProductModel CreateOrderProductModel(OrderProduct orderProduct) =>
            new OrderProductModel
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
}