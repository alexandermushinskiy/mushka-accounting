using System.Collections.Generic;
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
            int hashCode = 17;
            hashCode = hashCode * 23 + obj.ProductId.GetHashCode();
            hashCode = hashCode * 23 + obj.OrderId.GetHashCode();

            return hashCode;
        }
    }
}