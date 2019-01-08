using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrdersListResponseModel : ResponseModelBase
    {
        public IEnumerable<OrderListModel> Data { get; set; }
    }
}