using System;

namespace Mushka.Domain.Entities
{
    public class CorporateOrderProduct
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }
        
        public Guid CorporateOrderId { get; set; }
        public CorporateOrder CorporateOrder { get; set; }
    }
}