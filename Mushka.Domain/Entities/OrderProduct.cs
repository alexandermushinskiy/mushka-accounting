using System;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class OrderProduct : IEntityProduct
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }
    }
}