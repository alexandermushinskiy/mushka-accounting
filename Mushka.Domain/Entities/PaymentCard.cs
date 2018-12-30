using System;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class PaymentCard : IEntity
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Owner { get; set; }

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}