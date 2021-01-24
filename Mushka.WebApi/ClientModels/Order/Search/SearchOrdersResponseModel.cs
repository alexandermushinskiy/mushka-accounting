using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Order.Search
{
    public class SearchOrdersResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<OrderSummaryModel> Items { get; set; }
    }
}