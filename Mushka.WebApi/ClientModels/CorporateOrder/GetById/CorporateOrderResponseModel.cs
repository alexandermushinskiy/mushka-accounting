using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.CorporateOrder.GetById
{
    public class CorporateOrderResponseModel
    {
        public CorporateOrderModel Order { get; set; }
        public IEnumerable<CorporateOrderProductModel> Products { get; set; }
    }
}