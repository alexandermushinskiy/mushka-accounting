using System;
using System.Collections.Generic;

namespace Mushka.Domain.Entities
{
    public class ContactPerson
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Phones { get; set; }

        public string Email { get; set; }

        public string City { get; set; }
    }
}