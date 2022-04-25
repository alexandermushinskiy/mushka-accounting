using AutoMapper;
using Mushka.Domain.Models;
using Mushka.Domain.Strings;
using Mushka.WebApi.ClientModels.Order.Search;
using DateRangeModel = Mushka.Domain.Models.DateRange;

namespace Mushka.WebApi.Resolvers.Orders
{
    public class OrdersFilterRequestConverter : ITypeConverter<SearchOrdersRequestModel, SearchOrdersFilter>
    {
        public SearchOrdersFilter Convert(
            SearchOrdersRequestModel source,
            SearchOrdersFilter destination,
            ResolutionContext context)
        {
            return new SearchOrdersFilter
            {
                SearchKey = source.Query.SearchKey,
                OrderDate = new DateRangeModel(source.Query.FromDate, source.Query.ToDate),

                CurrentPage = source.Page.From,
                PageSize = source.Page.Size,

                SortKey = source.Sort.Key,
                IsAsc = source.Sort.Order == SortOrder.Asc
            };
        }
    }
}