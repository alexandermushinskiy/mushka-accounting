using System;

namespace Mushka.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string CardNumber { get; set; }
    }
}