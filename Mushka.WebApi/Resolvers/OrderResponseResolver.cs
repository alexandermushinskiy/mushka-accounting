using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;

namespace Mushka.WebApi.Resolvers
{
    public class OrderResponseResolver :
        IValueResolver<ValidationResponse<Order>, OrderResponseModel, OrderModel>,
        IValueResolver<ValidationResponse<IEnumerable<Order>>, OrdersResponseModel, IEnumerable<OrderModel>>
    {
        public OrderModel Resolve(
            ValidationResponse<Order> source,
            OrderResponseModel destination,
            OrderModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Order, OrderModel>(source.Result);

        public IEnumerable<OrderModel> Resolve(
            ValidationResponse<IEnumerable<Order>> source,
            OrdersResponseModel destination,
            IEnumerable<OrderModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Order, OrderModel>);
    }
}