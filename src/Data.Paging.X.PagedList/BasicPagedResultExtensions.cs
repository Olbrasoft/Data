using Olbrasoft.Data.Paging;
using Olbrasoft.Data.Paging.X.PagedList;
using System;
using X.PagedList;

namespace Olbrasoft.Extensions.Paging
{
    public static class BasicPagedResultExtensions
    {
        public static IPagedList<T> AsPagedList<T>(this IBasicPagedResult<T> source, IPageInfo paging)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (paging is null)
                throw new ArgumentNullException(nameof(paging));

            return new SimplePagedList<T>(source, paging.NumberOfSelectedPage, paging.PageSize, source.TotalCount);
        }
    }
}