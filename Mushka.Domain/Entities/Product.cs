using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Product : IEntity
    {
        public Product()
        {
            Sizes = new List<ProductSize>();
            Deliveries = new List<DeliveryProduct>();
            Orders = new List<OrderProduct>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public ICollection<ProductSize> Sizes { get; set; }

        public ICollection<DeliveryProduct> Deliveries { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        //public int TotalCount { get { return Sizes.Sum(s => s.Quantity); } }
    }
}