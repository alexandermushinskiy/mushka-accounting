using System;

namespace Mushka.WebApi.ClientModels.Order
{
    // TODO: Move each class to it's own file
    public class OrdersFilterRequestModel
    {
        public SearchOrdersFilterQuery Query { get; set; }
        public NavigationRequest Navigation { get; set; }
    }

    public class SearchOrdersFilterQuery
    {
        public SearchOrdersCustomer Customer { get; set; }
        public SearchOrdersOrder Order { get; set; }
    }

    public class SearchOrdersCustomer
    {
        public QueryLike Name { get; set; }
    }

    public class QueryLike
    {
        public string Like { get; set; }

        public static implicit operator string(QueryLike queryLike)
        {
            return queryLike?.Like;
        }
    }

    public class SearchOrdersOrder
    {
        public DateRange OrderDate { get; set; }
    }

    public class DateRange
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class NavigationRequest
    {
        public SortRequestModel Sort { get; set; }
        public PageRequestModel Page { get; set; }
    }

    public class SortRequestModel
    {
        public string Key { get; set; }
        public string Order { get; set; }
    }

    public class PageRequestModel
    {
        public int Size { get; set; }
        public int From { get; set; }
    }
}