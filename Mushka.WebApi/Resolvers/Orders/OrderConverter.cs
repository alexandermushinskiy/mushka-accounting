using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order.GetById;

namespace Mushka.WebApi.Resolvers.Orders
{
    public class OrderConverter :
        ITypeConverter<Order, OrderModel>,
        ITypeConverter<OrderProduct, OrderProductModel>,
        ITypeConverter<Customer, OrderCustomerModel>
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
                Region = source.Region,
                City = source.City
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

        public OrderCustomerModel Convert(Customer source, OrderCustomerModel destination, ResolutionContext context) =>
            new OrderCustomerModel
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Phone = source.Phone,
                Email = source.Email,
            };
    }
}