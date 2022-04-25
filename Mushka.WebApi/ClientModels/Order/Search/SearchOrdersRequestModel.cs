using System;
using Mushka.WebApi.ClientModels.Infrastructure.Navigation;

namespace Mushka.WebApi.ClientModels.Order.Search
{
    // TODO: Move each class to it's own file
    public class SearchOrdersRequestModel
    {
        public SearchOrdersQuery Query { get; set; }
        public SortRequestModel Sort { get; set; }
        public PageRequestModel Page { get; set; }
    }

    public class SearchOrdersQuery
    {
        public string SearchKey { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public abstract class SearchRequestModel<TQuery>
    {
        public TQuery Query { get; set; }
        public SortRequestModel Sort { get; set; }
        public PageRequestModel Page { get; set; }
    }
}