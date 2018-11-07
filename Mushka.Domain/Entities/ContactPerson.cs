using System;
using System.Collections.Generic;

namespace Mushka.Domain.Entities
{
    public class ContactPerson
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public ICollection<string> Phones { get; set; }

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}