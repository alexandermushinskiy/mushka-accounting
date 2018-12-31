using System;

namespace Mushka.Domain.Entities
{
    public class SupplyProductSize
    {
        public Guid ProductId { get; set; }
        public Guid SupplyId { get; set; }
        public SupplyProduct Product { get; set; }

        public Guid SizeId { get; set; }
        public Size Size { get; set; }

        public int Quantity { get; set; }
    }
}