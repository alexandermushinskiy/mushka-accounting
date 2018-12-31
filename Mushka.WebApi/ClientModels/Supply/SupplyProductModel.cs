using System;
using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyProductModel
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }
        
        public decimal CostForItem { get; set; }

        public IEnumerable<SupplyProductSizeModel> Sizes { get; set; }
    }
}