using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Comparers
{
    public class SupplyProductComparer : IEqualityComparer<SupplyProduct>
    {
        public bool Equals(SupplyProduct x, SupplyProduct y)
        {
            return x.ProductId == y.ProductId && x.SupplyId == y.SupplyId;
        }

        public int GetHashCode(SupplyProduct obj)
        {
            int hashCode = 17;
            hashCode = hashCode * 23 + obj.ProductId.GetHashCode();
            hashCode = hashCode * 23 + obj.SupplyId.GetHashCode();

            return hashCode;
        }
    }
}