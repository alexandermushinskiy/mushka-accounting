using System;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyProductModel
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }
        
        public decimal CostForItem { get; set; }

        public int Quantity { get; set; }
    }
}