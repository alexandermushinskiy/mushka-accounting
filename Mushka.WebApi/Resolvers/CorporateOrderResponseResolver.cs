using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder;

namespace Mushka.WebApi.Resolvers
{
    public class CorporateOrderResponseResolver :
        IValueResolver<OperationResult<CorporateOrder>, CorporateOrderResponseModel, CorporateOrderModel>
    {
        public CorporateOrderModel Resolve(
            OperationResult<CorporateOrder> source,
            CorporateOrderResponseModel destination,
            CorporateOrderModel destMember,
            ResolutionContext context) => source.Data == null ? null : Mapper.Map<CorporateOrder, CorporateOrderModel>(source.Data);
    }
}