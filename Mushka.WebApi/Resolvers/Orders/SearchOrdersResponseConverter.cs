using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Models;
using Mushka.WebApi.ClientModels.Order.Search;

namespace Mushka.WebApi.Resolvers.Orders
{
    public class SearchOrdersResponseConverter :
        ITypeConverter<OperationResult<ItemsList<OrderSummaryDto>>, SearchOrdersResponseModel>
    {
        public SearchOrdersResponseModel Convert(
            OperationResult<ItemsList<OrderSummaryDto>> source,
            SearchOrdersResponseModel destination,
            ResolutionContext context)
        {
            return new SearchOrdersResponseModel
            {
                Total = source.Data?.Total ?? 0,
                Items = source.Data?.Items.Select(Mapper.Map<OrderSummaryDto, OrderSummaryModel>) ?? Enumerable.Empty<OrderSummaryModel>()
            };
        }
    }
}