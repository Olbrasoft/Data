using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Data.Paging.X.PagedList.Tests")]

namespace Olbrasoft.Data.Paging.X.PagedList;

internal class SimplePagedList<T> : BasePagedList<T>
{
    public SimplePagedList(IEnumerable<T> subSet, int pageNumber, int pageSize, int totalItemCount) : base(pageNumber, pageSize, totalItemCount)
    {
        if (subSet is null)
            throw new ArgumentNullException(nameof(subSet));

        if (Subset is null)
            throw new ArgumentNullException(nameof(Subset));

        Subset.AddRange(subSet);
    }
}