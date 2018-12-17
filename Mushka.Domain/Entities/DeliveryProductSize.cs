using System;

namespace Mushka.Domain.Entities
{
    public class DeliveryProductSize
    {
        public Guid ProductId { get; set; }
        public Guid DeliveryId { get; set; }
        public DeliveryProduct Product { get; set; }

        public Guid SizeId { get; set; }
        public Size Size { get; set; }

        public int Quantity { get; set; }
    }
}