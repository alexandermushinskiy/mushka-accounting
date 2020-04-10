using System;
using System.Linq.Expressions;

namespace Mushka.Domain.Specifications
{
    public abstract class Specification<TEntity>
    {
        public static readonly Specification<TEntity> All = new IdentitySpecification<TEntity>();

        public bool IsSatisfied(TEntity entity)
        {
            Func<TEntity, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public abstract Expression<Func<TEntity, bool>> ToExpression();

        public Specification<TEntity> And(Specification<TEntity> specification)
        {
            if (this == All)
            {
                return specification;
            }

            if (specification == All)
            {
                return this;
            }

            return new AndSpecification<TEntity>(this, specification);
        }

        public Specification<TEntity> Or(Specification<TEntity> specification)
        {
            if (this == All || specification == All)
            {
                return All;
            }

            return new OrSpecification<TEntity>(this, specification);
        }

        public Specification<TEntity> Not()
        {
            return new NotSpecification<TEntity>(this);
        }
    }
}