namespace Olbrasoft.Extensions.Paging;

public static class PagedEnumerableExtensions
{
    public static IPagedList<T> AsPagedList<T>(this IPagedEnumerable<T> source, IPageInfo paging)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (paging is null)
            throw new ArgumentNullException(nameof(paging));

        return new SimplePagedList<T>(source, paging.NumberOfSelectedPage, paging.PageSize, source.TotalCount);
    }
}