using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Category : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsSizeRequired { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}