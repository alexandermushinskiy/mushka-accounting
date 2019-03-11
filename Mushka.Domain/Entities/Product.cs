using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Product : IEntity
    {
        public Product()
        {
            Supplies = new List<SupplyProduct>();
            Orders = new List<OrderProduct>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Quantity { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid? SizeId { get; set; }
        public Size Size { get; set; }
        
        public ICollection<SupplyProduct> Supplies { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }

        public ICollection<ExhibitionProduct> Exhibitions { get; set; }
    }
}