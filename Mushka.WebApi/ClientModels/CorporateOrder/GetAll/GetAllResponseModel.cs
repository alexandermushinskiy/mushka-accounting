using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.CorporateOrder.GetAll
{
    public class GetAllResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<CorporateOrderSummaryModel> Items { get; set; }
    }
}