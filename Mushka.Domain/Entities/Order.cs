using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Order : IEntity
    {
        public Order()
        {
            Products = new List<OrderProduct>();
        }

        public Guid Id { get; set; }

        public string Number { get; set; }

        public DateTime OrderDate { get; set; }
        
        public decimal Cost { get; set; }
        
        public PaymentMethod CostMethod { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<OrderProduct> Products { get; set; }
    }
}