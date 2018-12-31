using System;
using System.Collections.Generic;

namespace Mushka.Domain.Entities
{
    public class SupplyProduct
    {
        public Guid SupplyId { get; set; }
        public Supply Supply { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public IEnumerable<SupplyProductSize> ProductSizes { get; set; }

        public decimal CostForItem { get; set; }
    }
}