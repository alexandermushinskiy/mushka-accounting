using System;
using System.Collections.Generic;

namespace Mushka.Domain.Entities
{
    public class Supplier
    {
        public Supplier()
        {
            Deliveries = new List<Delivery>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Notes { get; set; }

        public ContactPerson ContactPerson { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }
    }
}