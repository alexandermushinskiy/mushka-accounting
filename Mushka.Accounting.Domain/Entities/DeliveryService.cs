using System;

namespace Mushka.Accounting.Domain.Entities
{
    public class DeliveryService
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public string Notes { get; set; }

        public Delivery Delivery { get; set; }
    }
}