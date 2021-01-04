using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Mushka.Infrastructure.DataAccess.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> source, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                source = includes.Aggregate(source, (current, include) => current.Include(include));
            }

            return source;
        }

        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> source, params string[] includes) where T : class
        {
            if (includes != null)
            {
                source = includes.Aggregate(source, (current, include) => current.Include(include));
            }

            return source;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string columnName, bool isAscending = true)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

            MemberExpression property = Expression.Property(parameter, columnName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                new Type[] { source.ElementType, property.Type },
                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}