using System.Collections.Generic;
using Mushka.Core;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Comparers
{
    public class ExhibitionProductComparer : IEqualityComparer<ExhibitionProduct>
    {
        public bool Equals(ExhibitionProduct x, ExhibitionProduct y)
        {
            return x.ProductId == y.ProductId && x.ExhibitionId == y.ExhibitionId;
        }

        public int GetHashCode(ExhibitionProduct obj)
        {
            return HashCodeGenerator.GetFromValues(new { obj.ProductId, obj.ExhibitionId });
        }
    }
}