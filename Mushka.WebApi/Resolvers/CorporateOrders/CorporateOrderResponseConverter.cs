using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder.GetById;

namespace Mushka.WebApi.Resolvers.CorporateOrders
{
    public class CorporateOrderResponseConverter :
        ITypeConverter<OperationResult<CorporateOrder>, CorporateOrderResponseModel>
    {
        public CorporateOrderResponseModel Convert(
            OperationResult<CorporateOrder> source,
            CorporateOrderResponseModel destination,
            ResolutionContext context)
        {
            return new CorporateOrderResponseModel
            {
                Order = context.Mapper.Map<CorporateOrder, CorporateOrderModel>(source.Data),
                Products = source.Data.Products.Select(x => context.Mapper.Map<CorporateOrderProduct, CorporateOrderProductModel>(x))
            };
        }
    }
}