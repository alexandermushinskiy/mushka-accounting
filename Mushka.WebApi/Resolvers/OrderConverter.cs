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
                Profit = source.Profit,
                IsWholesale = source.IsWholesale,
                Notes = source.Notes,
                Region = source.Customer.Region,
                City = source.Customer.City,
                Customer = CreateCustomerModel(source.Customer),
                Products = source.Products.Select(CreateOrderProductModel)
            };

        public OrderProductModel Convert(OrderProduct source, OrderProductModel destination, ResolutionContext context) =>
            CreateOrderProductModel(source);

        private static OrderCustomerModel CreateCustomerModel(Customer customer) =>
            new OrderCustomerModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                Email = customer.Email,
            };
        

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