using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Supply : IEntity
    {
        public Supply()
        {
            Products = new List<SupplyProduct>();
        }

        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime ReceivedDate { get; set; }
        
        public decimal Cost { get; set; }

        public PaymentMethod CostMethod { get; set; }

        public decimal? Prepayment { get; set; }

        public PaymentMethod? PrepaymentMethod { get; set; }

        public decimal? DeliveryCost { get; set; }

        public PaymentMethod? DeliveryCostMethod { get; set; }

        public decimal? BankFee { get; set; }

        public decimal TotalCost { get; set; }

        public string Notes { get; set; }

        public ICollection<SupplyProduct> Products { get; set; }

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}