using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Size : IEntity
    {
        public Size()
        {
            Products = new List<Product>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}