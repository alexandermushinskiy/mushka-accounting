using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;
using Mushka.WebApi.Extensions;

namespace Mushka.WebApi.Resolvers
{
    public class OrderRequestConverter : ITypeConverter<OrderRequestModel, Order>
    {
        private readonly IGuidProvider guidProvider;

        public OrderRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Order Convert(OrderRequestModel source, Order destination, ResolutionContext context)
        {
            var orderId = context.GetId() ?? guidProvider.NewGuid();
            var customerId = source.Customer.Id ?? guidProvider.NewGuid();
            //var customerId = guidProvider.NewGuid();

            return new Order
            {
                Id = orderId,
                OrderDate = source.OrderDate,
                Number = source.Number,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                Discount = source.Discount == 0 ? null : source.Discount,
                Profit = source.Profit,
                IsWholesale = source.IsWholesale,
                Notes = source.Notes,
                Customer = CreateCustomer(customerId, source),
                Products = source.Products.Select(prod => CreateOrderProduct(orderId, prod)).ToList()
            };
        }

        private static Customer CreateCustomer(Guid customerId, OrderRequestModel requestModel) =>
            new Customer
            {
                Id = customerId,
                FirstName = requestModel.Customer.FirstName,
                LastName = requestModel.Customer.LastName,
                Phone = requestModel.Customer.Phone,
                Email = requestModel.Customer.Email,
                Region = requestModel.Region,
                City = requestModel.City
            };

        private static OrderProduct CreateOrderProduct(Guid orderId, OrderProductRequestModel requestModel) =>
            new OrderProduct
            {
                OrderId = orderId,
                ProductId = requestModel.ProductId,
                Quantity = requestModel.Quantity,
                UnitPrice = requestModel.UnitPrice,
                CostPrice = requestModel.CostPrice
            };
    }
}