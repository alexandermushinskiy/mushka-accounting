using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder.GetAll;

namespace Mushka.WebApi.Resolvers.CorporateOrders
{
    public class GetAllResponseResolver :
        ITypeConverter<OperationResult<IEnumerable<CorporateOrder>>, GetAllResponseModel>
    {
        public GetAllResponseModel Convert(OperationResult<IEnumerable<CorporateOrder>> source, GetAllResponseModel destination, ResolutionContext context)
        {
            return new GetAllResponseModel
            {
                Total = source.Data?.Count() ?? 0,
                Items = source.Data?.Select(Mapper.Map<CorporateOrder, CorporateOrderSummaryModel>) ?? Enumerable.Empty<CorporateOrderSummaryModel>()
            };
        }
    }
}