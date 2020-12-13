using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;

namespace Mushka.WebApi.Resolvers
{
    public class OrderResponseResolver :
        IValueResolver<OperationResult<Order>, OrderResponseModel, OrderModel>
    {
        public OrderModel Resolve(
            OperationResult<Order> source,
            OrderResponseModel destination,
            OrderModel destMember,
            ResolutionContext context) => source.Data == null ? null : Mapper.Map<Order, OrderModel>(source.Data);
    }
}