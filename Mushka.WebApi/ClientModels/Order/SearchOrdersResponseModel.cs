using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Order
{
    public class SearchOrdersResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<OrderListModel> Items { get; set; }
    }

    public class ErrorResponseModel
    {
        public IEnumerable<string> Errors { get; set; }
    }
}