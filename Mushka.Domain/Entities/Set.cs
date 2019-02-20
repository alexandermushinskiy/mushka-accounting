using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Set// : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public ICollection<SetProduct> Products { get; set; }
    }
}