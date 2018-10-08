using System;
using System.Collections.Generic;
using Mushka.Accounting.Domain.Extensibility.Entities;

namespace Mushka.Accounting.Domain.Entities
{
    public class Delivery : IEntity
    {
        public Guid Id { get; set; }
        public string BatchNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TransferFee { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsDraft { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public ICollection<DeliveryProduct> Products { get; set; }
        public ICollection<DeliveryService> Services { get; set; }
    }
}