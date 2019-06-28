using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Customer : IEntity, IEquatable<Customer>
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
        
        public ICollection<Order> Orders { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public bool Equals(Customer other)
        {
            return FirstName == other.FirstName &&
                   LastName == other.LastName &&
                   Phone == other.Phone;
        }
    }
}