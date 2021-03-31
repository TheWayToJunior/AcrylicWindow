using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Helpers
{
    public static class CollectionHelper
    {
        public static IQueryable<TModel> Pagination<TModel>(this IQueryable<TModel> queryable, int index, int pageSize)
        {
            return queryable
                .Skip((index - 1) * pageSize)
                .Take(pageSize);
        }

        public static IEnumerable<TModel> Pagination<TModel>(this IEnumerable<TModel> enumerable, int index, int pageSize)
        {
            return enumerable
                .Skip((index - 1) * pageSize)
                .Take(pageSize);
        }

        public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method)
        {
            return await Task.WhenAll(source.Select(async s => await method(s)));
        }

        public static void TryAdd<TModel>(this ICollection<TModel> source, TModel model)
        {
            if (!source.Contains(model))
            {
                source.Add(model);
            }
        }
    }
}
