using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderProductsResponseModel : ResponseModelBase
    {
        public IEnumerable<OrderProductModel> Data { get; set; }
    }
}