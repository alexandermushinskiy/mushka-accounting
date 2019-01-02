using System;
using Mushka.WebApi.Filters;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyProductRequestModel
    {
        [RequireNonDefault]
        public Guid ProductId { get; set; }
        
        [RequireNonDefault]
        public int Quantity { get; set; }

        [RequireNonDefault]
        public decimal CostPerItem { get; set; }
    }
}