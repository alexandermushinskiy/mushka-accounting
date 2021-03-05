using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supply.Describe
{
    public class DescribeSupplyResponseModel
    {
        public SupplyModel Supply { get; set; }

        public IEnumerable<SupplyProductModel> Products { get; set; }
    }
}