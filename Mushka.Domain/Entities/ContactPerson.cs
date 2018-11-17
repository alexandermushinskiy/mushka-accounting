using System;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class ContactPerson : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public string Email { get; set; }

        public string Phones { get; set; }

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}