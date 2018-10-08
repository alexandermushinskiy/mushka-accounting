using System;
using System.Collections.Generic;

namespace Mushka.Accounting.Domain.Entities
{
    public class DeliveryProduct
    {
        public Guid DeliveryId { get; set; }
        public Delivery Delivery { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Amount { get; set; }
        public int CostPerItem { get; set; }
        public string Notes { get; set; }
        
        public ICollection<ProductSizeItem> SizeItems { get; set; }

        public int TotalCost => Amount * CostPerItem;
    }
}