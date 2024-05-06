using Hexagonal.Application.Common.Traits;
using System.Linq.Expressions;

namespace Hexagonal.Application.Common.Extensions
{
    public static class QuerableExtensions
    {
        /// <summary>
        /// Paginate a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="term"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> PaginateList<T>(this IQueryable<T> list, string? term, int? skip, int? top, Expression<Func<T, bool>> expression)
        {
            if (!string.IsNullOrEmpty(term) && term.Length > 2)
            {
                list = list.Where(expression);
            }

            if (skip.HasValue) list = list.Skip(skip.Value);
            if (top.HasValue) list = list.Take(top.Value);

            return list;
        }

        /// <summary>
        /// Filters entities that supports soft delete
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns>filtered query based on deleted flag</returns>
        public static IQueryable<TEntity> FilterSoftDelete<TEntity>(this IQueryable<TEntity> query)
            where TEntity : ISoftDelete
        {
            return query.Where(e => e.Deleted == null);
        }
    }
}
