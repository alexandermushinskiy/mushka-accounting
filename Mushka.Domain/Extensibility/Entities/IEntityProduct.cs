using System;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Entities
{
    public interface IEntityProduct
    {
        Guid ProductId { get; set; }
        Product Product { get; set; }

        int Quantity { get; set; }

        decimal UnitPrice { get; set; }

        decimal CostPrice { get; set; }
    }
}