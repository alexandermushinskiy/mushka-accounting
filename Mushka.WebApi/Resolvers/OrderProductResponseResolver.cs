using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;

namespace Mushka.WebApi.Resolvers
{
    public class OrderProductResponseResolver :
        IValueResolver<ValidationResponse<IEnumerable<OrderProduct>>, OrderProductsResponseModel, IEnumerable<OrderProductModel>>
    {
        public IEnumerable<OrderProductModel> Resolve(
            ValidationResponse<IEnumerable<OrderProduct>> source,
            OrderProductsResponseModel destination,
            IEnumerable<OrderProductModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<OrderProduct, OrderProductModel>);
    }
}