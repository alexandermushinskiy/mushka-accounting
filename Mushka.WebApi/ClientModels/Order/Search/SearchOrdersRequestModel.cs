using Mushka.WebApi.ClientModels.Infrastructure.Navigation;
using Mushka.WebApi.ClientModels.Infrastructure.Queries;

namespace Mushka.WebApi.ClientModels.Order.Search
{
    // TODO: Move each class to it's own file
    public class SearchOrdersRequestModel
    {
        public SearchOrdersQuery Query { get; set; }
        public NavigationRequest Navigation { get; set; }
    }

    public class SearchOrdersQuery
    {
        public SearchOrdersCustomer Customer { get; set; }
        public SearchOrdersOrder Order { get; set; }
    }

    public class SearchOrdersCustomer
    {
        public QueryLike Name { get; set; }
    }

    public class SearchOrdersOrder
    {
        public QueryBetween OrderDate { get; set; }
    }
}