using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Delivery : IEntity
    {
        public Delivery()
        {
            Products = new List<DeliveryProduct>();
        }

        public Guid Id { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal Cost { get; set; }

        public decimal TransferFee { get; set; }

        public decimal BankFee { get; set; }

        public ICollection<DeliveryProduct> Products { get; set; }

        //public Guid SupplierId { get; set; }
        //public Supplier Supplier { get; set; }
    }
}