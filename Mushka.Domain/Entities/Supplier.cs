using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Supplier : IEntity
    {
        public Supplier()
        {
            Supplies = new List<Supply>();
            ContactPersons = new List<ContactPerson>();
            PaymentCards = new List<PaymentCard>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }
        
        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public string Service { get; set; }
        
        public ICollection<ContactPerson> ContactPersons { get; set; }
        
        public ICollection<Supply> Supplies { get; set; }
        
        public ICollection<PaymentCard> PaymentCards { get; set; }
    }
}