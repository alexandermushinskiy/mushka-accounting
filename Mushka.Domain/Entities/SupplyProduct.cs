using System;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class SupplyProduct : IEntityProduct
    {
        public Guid SupplyId { get; set; }
        public Supply Supply { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }
    }
}