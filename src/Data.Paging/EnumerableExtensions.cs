using Olbrasoft.Data.Paging;
using System.Collections.Generic;

namespace Olbrasoft.Extensions.Paging
{
    public static class EnumerableExtensions
    {
        public static IPagedEnumerable<T> AsPagedEnumerable<T>(this IEnumerable<T> items)
        {
            return new PagedEnumerable<T>(items);
        }

        public static IPagedResult<T> AsPagedResult<T>(this IEnumerable<T> items)
        {
            return new PagedResult<T>(items);
        }
    }
}