using System;
using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Delivery
{
    public class DeliveryProductModel
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }
        
        public decimal PriceForItem { get; set; }

        public IEnumerable<DeliveryProductSizeModel> Sizes { get; set; }
    }
}