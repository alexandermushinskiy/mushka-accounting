using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Accounting.Domain.Extensibility.Entities;

namespace Mushka.Accounting.Domain.Entities
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreatedOn { get; set; }
        //public int DeliveriesNumber { get; set; }
        //public DateTime LastDeliveryDate { get; set; }
        //public int LastDeliveryCount { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        //public ICollection<DeliveryProduct> Deliveries { get; set; }
        //public ICollection<ProductSizeItem> SizeItems { get; set; }

        public ICollection<ProductSize> Sizes { get; set; }

        //public int TotalCount { get { return Sizes.Sum(s => s.Quantity); } }
    }
}