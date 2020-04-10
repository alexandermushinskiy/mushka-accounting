using System;
using System.Linq.Expressions;

namespace Mushka.Domain.Specifications
{
    internal sealed class IdentitySpecification<TEntity> : Specification<TEntity>
    {
        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return x => true;
        }
    }
}