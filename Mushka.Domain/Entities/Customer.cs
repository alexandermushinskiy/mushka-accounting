using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Customer : IEntity
    {
        public Customer()
        {
            Orders = new List<Order>();
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}