using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.CorporateOrder
{
    public class CorporateOrdersListResponseModel : ResponseModelBase
    {
        public IEnumerable<CorporateOrderListModel> Data { get; set; }
    }
}