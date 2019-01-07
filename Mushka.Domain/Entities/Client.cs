using System;
using System.Collections.Generic;

namespace Mushka.Domain.Entities
{
    public class Client
    {
        public Client()
        {
            Orders = new List<Order>();
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}