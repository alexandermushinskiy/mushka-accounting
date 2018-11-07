using System;
using System.Collections.Generic;

namespace Mushka.Domain.Entities
{
    public class Supplier
    {
        public Supplier()
        {
            Deliveries = new List<Delivery>();
            ContactPersons = new List<ContactPerson>();
            Payments = new List<Payment>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Notes { get; set; }

        public ICollection<ContactPerson> ContactPersons { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }
    }
}