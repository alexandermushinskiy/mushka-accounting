using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder;

namespace Mushka.WebApi.Resolvers
{
    public class CorporateOrderResponseResolver :
        IValueResolver<ValidationResponse<CorporateOrder>, CorporateOrderResponseModel, CorporateOrderModel>
    {
        public CorporateOrderModel Resolve(
            ValidationResponse<CorporateOrder> source,
            CorporateOrderResponseModel destination,
            CorporateOrderModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<CorporateOrder, CorporateOrderModel>(source.Result);
    }
}