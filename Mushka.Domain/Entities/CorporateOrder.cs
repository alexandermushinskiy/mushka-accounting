using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class CorporateOrder : IEntity
    {
        public CorporateOrder()
        {
            Products = new List<CorporateOrderProduct>();
        }

        public Guid Id { get; set; }

        public string Number { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Cost { get; set; }

        public PaymentMethod CostMethod { get; set; }

        public decimal? Prepayment { get; set; }

        public PaymentMethod? PrepaymentMethod { get; set; }

        public decimal? DeliveryCost { get; set; }

        public PaymentMethod? DeliveryCostMethod { get; set; }

        public int? Tax { get; set; }

        public decimal Profit { get; set; }

        public string CompanyName { get; set; }

        public string ContactPerson { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Notes { get; set; }

        public ICollection<CorporateOrderProduct> Products { get; set; }
    }
}