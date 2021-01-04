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
        public string Criteria { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
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