using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mushka.Domain.Specifications
{
    internal sealed class AndSpecification<TEntity> : Specification<TEntity>
    {
        private readonly Specification<TEntity> left;
        private readonly Specification<TEntity> right;

        public AndSpecification(Specification<TEntity> left, Specification<TEntity> right)
        {
            this.left = left;
            this.right = right;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            Expression<Func<TEntity, bool>> leftExpression = left.ToExpression();
            Expression<Func<TEntity, bool>> rightExpression = right.ToExpression();

            BinaryExpression andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);

            return Expression.Lambda<Func<TEntity, bool>>(andExpression, leftExpression.Parameters.Single());
        }
    }
}