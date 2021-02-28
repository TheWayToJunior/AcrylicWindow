using System.Collections.Generic;
using System.Linq;

namespace AcrylicWindow.Client.Core.Helpers
{
    public static class PaginationHelper
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
    }
}
