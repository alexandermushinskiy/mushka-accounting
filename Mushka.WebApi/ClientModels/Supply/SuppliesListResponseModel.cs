using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SuppliesListResponseModel : ResponseModelBase
    {
        public IEnumerable<SupplyListModel> Data { get; set; }
    }
}