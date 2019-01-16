using System;
using Mushka.WebApi.Filters;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderProductRequestModel
    {
        [RequireNonDefault]
        public Guid ProductId { get; set; }
        
        [RequireNonDefault]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [RequireNonDefault]
        public decimal CostPrice { get; set; }
    }
}