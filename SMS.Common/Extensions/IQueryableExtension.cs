using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace SMS.Common.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<TSource> ApplySorting<TSource>(this IQueryable<TSource> query, string keySelector, bool isDescending = false)
        {
            if (isDescending)
                return query.OrderBy($"{keySelector} descending");

            return query.OrderBy(keySelector);
        }

        public static IQueryable<TSource> ApplySorting<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, bool isDescending = false)
        {
            if (isDescending)
                return query.OrderByDescending(keySelector);

            return query.OrderBy(keySelector);
        }

        public static IQueryable<TSource> OrderByIf<TSource>(this IQueryable<TSource> query, bool condition, string keySelector, bool isDescending = true)
        {
            if (condition)
            {
                return query.ApplySorting(keySelector, isDescending);
            }

            return query;
        }

        public static IQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, TKey>> keySelector, bool isDescending = true)
        {
            if (condition)
            {
                return query.ApplySorting(keySelector, isDescending);
            }

            return query;
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> query, bool condition, string predicate, params object[] args)
        {
            if (condition)
            {
                return query.Where($"{predicate}.Contains(@0)", args);
            }

            return query;
        }

        public static IQueryable<TSource> PageBy<TSource>(this IQueryable<TSource> query, int pageNumber, int pageSize)
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<TSource> WhereIf<TSource, TKey>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, TKey>> keySelector)
        {
            if (condition)
            {
                return query.Where(keySelector);
            }

            return query;
        }

        public static IQueryable<TSource> WhereFilter<TSource, TObject>(this IQueryable<TSource> query, TObject request)
        {
            var properties = request!.GetType().GetProperties().Select(x => new { x.Name, value = x.GetValue(request, null) }).Where(x => x.value != null).ToList();

            foreach (var prop in properties)
            {
                // replace with reflection
                //PropertyInfo propertyInfoObj = request.GetType().GetProperty(prop.Name);
                // query = query.Where(x => x.GetType().GetProperty(prop.Name).GetValue(x,null) == prop.value );
                query = query.WhereIf(true, prop.Name, prop.value!);
            }

            return query;
        }
        public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, string searchTerm, Func<T, bool> filterCondition = null)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(item => item.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (filterCondition != null)
            {
                query = query.Where(filterCondition).AsQueryable();
            }

            return query;
        }
    }
}
