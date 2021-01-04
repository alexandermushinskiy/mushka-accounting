using AutoMapper;
using Mushka.Domain.Models;
using Mushka.WebApi.ClientModels.Order;

namespace Mushka.WebApi.Resolvers
{
    public class OrdersFilterRequestConverter : ITypeConverter<OrdersFilterRequestModel, SearchOrdersFilter>
    {
        public SearchOrdersFilter Convert(OrdersFilterRequestModel source, SearchOrdersFilter destination, ResolutionContext context)
        {
            return new SearchOrdersFilter
            {
                Criteria = source.Query.Criteria,
                FromDate = source.Query.FromDate,
                ToDate = source.Query.ToDate,

                CurrentPage = source.Navigation.Page.From,
                PageSize = source.Navigation.Page.Size,

                SortKey = source.Navigation.Sort.Key,
                SortOrder = source.Navigation.Sort.Order
            };
        }
    }
}