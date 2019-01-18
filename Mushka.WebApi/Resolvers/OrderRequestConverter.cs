using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;

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
            var orderId = guidProvider.NewGuid();
            var customerId = guidProvider.NewGuid();
            
            return new Order
            {
                Id = orderId,
                OrderDate = source.OrderDate,
                Number = source.Number,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                Profit = source.Profit,
                Notes = source.Notes,
                Customer = CreateCustomer(customerId, source),
                Products = source.Products.Select(prod => CreateDeliveryProduct(orderId, prod)).ToList()
            };
        }

        private static Customer CreateCustomer(Guid customerId, OrderRequestModel requestModel) =>
            new Customer
            {
                Id = customerId,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                Phone = requestModel.Phone,
                Email = requestModel.Email,
                Region = requestModel.Region,
                City = requestModel.City
            };

        private static OrderProduct CreateDeliveryProduct(Guid orderId, OrderProductRequestModel requestModel) =>
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