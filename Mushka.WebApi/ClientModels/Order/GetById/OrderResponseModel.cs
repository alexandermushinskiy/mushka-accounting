using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Order.GetById
{
    public class OrderResponseModel
    {
        public OrderModel Order { get; set; }
        public OrderCustomerModel Customer { get; set; }
        public IEnumerable<OrderProductModel> Products { get; set; }
    }
}