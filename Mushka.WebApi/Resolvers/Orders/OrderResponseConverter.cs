using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order.GetById;

namespace Mushka.WebApi.Resolvers.Orders
{
    public class OrderResponseConverter : ITypeConverter<OperationResult<Order>, OrderResponseModel>
    {
        public OrderResponseModel Convert(OperationResult<Order> source, OrderResponseModel destination, ResolutionContext context)
        {
            return new OrderResponseModel
            {
                Order = context.Mapper.Map<Order, OrderModel>(source.Data),
                Customer = context.Mapper.Map<Customer, OrderCustomerModel>(source.Data.Customer),
                Products = source.Data.Products.Select(x => context.Mapper.Map<OrderProduct, OrderProductModel>(x))
            };
        }
    }
}