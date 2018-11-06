using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderResponseModel : ResponseModelBase
    {
        public OrderModel Data { get; set; }
    }

    public class OrdersResponseModel : ResponseModelBase
    {
        public IEnumerable<OrderModel> Data { get; set; }
    }
}