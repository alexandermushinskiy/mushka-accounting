using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Supplier : IEntity
    {
        public Supplier()
        {
            Deliveries = new List<Delivery>();
            ContactPersons = new List<ContactPerson>();
            //Phones = new List<PhoneNumber>();
            //Payments = new List<Payment>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Phones { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<ContactPerson> ContactPersons { get; set; }

        //public ICollection<PhoneNumber> Phones { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

        //public ICollection<Payment> Payments { get; set; }
    }
}