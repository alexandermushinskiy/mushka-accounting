using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyResponseModel : ResponseModelBase
    {
        public SupplyModel Data { get; set; }
    }

    public class SuppliesResponseModel : ResponseModelBase
    {
        public IEnumerable<SupplyModel> Data { get; set; }
    }
}