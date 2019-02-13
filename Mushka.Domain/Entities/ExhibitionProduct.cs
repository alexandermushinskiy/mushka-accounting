using System;

namespace Mushka.Domain.Entities
{
    public class ExhibitionProduct
    {
        public Guid ExhibitionId { get; set; }
        public Exhibition Exhibition { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }
    }
}