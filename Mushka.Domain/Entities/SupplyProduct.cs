using System;

namespace Mushka.Domain.Entities
{
    public class SupplyProduct
    {
        public Guid SupplyId { get; set; }
        public Supply Supply { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal CostForItem { get; set; }
    }
}