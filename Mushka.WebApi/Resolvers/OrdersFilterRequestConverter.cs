using AutoMapper;
using Mushka.Domain.Models;
using Mushka.WebApi.ClientModels.Order;
using DateRangeModel = Mushka.Domain.Models.DateRange;

namespace Mushka.WebApi.Resolvers
{
    public class OrdersFilterRequestConverter : ITypeConverter<OrdersFilterRequestModel, SearchOrdersFilter>
    {
        public SearchOrdersFilter Convert(OrdersFilterRequestModel source, SearchOrdersFilter destination, ResolutionContext context)
        {
            return new SearchOrdersFilter
            {
                CustomerName = source.Query.Customer?.Name,
                OrderDate = new DateRangeModel(source.Query.Order.OrderDate.From, source.Query.Order.OrderDate.To),

                CurrentPage = source.Navigation.Page.From,
                PageSize = source.Navigation.Page.Size,

                SortKey = source.Navigation.Sort.Key,
                SortOrder = source.Navigation.Sort.Order
            };
        }
    }
}