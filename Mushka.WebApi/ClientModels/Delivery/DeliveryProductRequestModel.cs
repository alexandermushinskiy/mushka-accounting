using System;
using Mushka.WebApi.Filters;

namespace Mushka.WebApi.ClientModels.Delivery
{
    public class DeliveryProductRequestModel
    {
        [RequireNonDefault]
        public Guid ProductId { get; set; }

        [RequireNonDefault]
        public Guid SizeId { get; set; }

        [RequireNonDefault]
        public int Quantity { get; set; }

        [RequireNonDefault]
        public decimal PriceForItem { get; set; }
    }
}