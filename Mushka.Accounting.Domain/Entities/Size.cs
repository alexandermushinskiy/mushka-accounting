using System;
using System.Collections.Generic;
using Mushka.Accounting.Domain.Extensibility.Entities;

namespace Mushka.Accounting.Domain.Entities
{
    public class Size : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<ProductSize> Products { get; set; }
    }
}