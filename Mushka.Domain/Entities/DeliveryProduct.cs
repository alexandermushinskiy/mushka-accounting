using System;
using System.Collections.Generic;

namespace Mushka.Domain.Entities
{
    public class DeliveryProduct
    {
        public Guid DeliveryId { get; set; }
        public Delivery Delivery { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public IEnumerable<DeliveryProductSize> ProductSizes { get; set; }

        public decimal PriceForItem { get; set; }
    }
}