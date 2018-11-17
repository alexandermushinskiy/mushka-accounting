using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Comparers
{
    public class EntityComparer<TEntity> : IEqualityComparer<TEntity> where TEntity : IEntity
    {
        public bool Equals(TEntity x, TEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(TEntity obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}