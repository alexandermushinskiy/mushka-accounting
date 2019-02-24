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
    }
}