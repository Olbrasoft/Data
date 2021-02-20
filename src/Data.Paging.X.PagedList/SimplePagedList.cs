using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using X.PagedList;

[assembly: InternalsVisibleTo("Data.Paging.X.PagedList.Tests")]

namespace Olbrasoft.Data.Paging.X.PagedList
{
    internal class SimplePagedList<T> : BasePagedList<T>
    {
        public SimplePagedList(IEnumerable<T> subSet, int pageNumber, int pageSize, int totalItemCount) : base(pageNumber, pageSize, totalItemCount)
        {
            if (subSet is null)
                throw new ArgumentNullException(nameof(subSet));

            Subset.AddRange(subSet);
        }
    }
}