using System;

namespace Mushka.Domain.Entities
{
    public class DeliveryProduct
    {
        public Guid DeliveryId { get; set; }
        public Delivery Delivery { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid SizeId { get; set; }
        public Size Size { get; set; }

        public int Quantity { get; set; }

        public decimal PriceForItem { get; set; }
    }
}