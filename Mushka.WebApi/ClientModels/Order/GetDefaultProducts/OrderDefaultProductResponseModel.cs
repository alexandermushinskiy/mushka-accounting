using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Order.GetDefaultProducts
{
    public class OrderDefaultProductResponseModel
    {
        public IEnumerable<OrderDefaultProductModel> Items { get; set; }
    }
}
