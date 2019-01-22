using System.Collections.Generic;
using Mushka.Core;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Comparers
{
    public class OrderProductComparer : IEqualityComparer<OrderProduct>
    {
        public bool Equals(OrderProduct x, OrderProduct y)
        {
            return x.ProductId == y.ProductId && x.OrderId == y.OrderId;
        }

        public int GetHashCode(OrderProduct obj)
        {
            return HashCodeGenerator.GetFromValues(new { obj.ProductId , obj.OrderId });
        }
    }
}