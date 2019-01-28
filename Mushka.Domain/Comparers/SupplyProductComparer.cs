using System.Collections.Generic;
using Mushka.Core;
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
            return HashCodeGenerator.GetFromValues(new { obj.ProductId, obj.SupplyId });
        }
    }
}