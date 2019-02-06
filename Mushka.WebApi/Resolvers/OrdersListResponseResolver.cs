using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;

namespace Mushka.WebApi.Resolvers
{
    public class OrdersListResponseResolver : IValueResolver<ValidationResponse<IEnumerable<Order>>, OrdersListResponseModel, IEnumerable<OrderListModel>>
    {
        public IEnumerable<OrderListModel> Resolve(
            ValidationResponse<IEnumerable<Order>> source,
            OrdersListResponseModel destination,
            IEnumerable<OrderListModel> destMember,
            ResolutionContext context)
        {
            return source.Result?.Select(order => new OrderListModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Number = order.Number,
                Cost = order.Cost,
                Address = $"{order.Customer.Region}, {order.Customer.City}",
                CustomerName = $"{order.Customer.FirstName} {order.Customer.LastName}",
                ProductsCount = order.Products.Sum(p => p.Quantity)
            });
        }
    }
}