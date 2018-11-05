using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Size : IEntity
    {
        public Size()
        {
            Products = new List<ProductSize>();
            DeliveryProducts = new List<DeliveryProduct>();
            OrderProducts = new List<OrderProduct>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ProductSize> Products { get; set; }

        public IEnumerable<DeliveryProduct> DeliveryProducts { get; set; }

        public IEnumerable<OrderProduct> OrderProducts { get; set; }
    }
}