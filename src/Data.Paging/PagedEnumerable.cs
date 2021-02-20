using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Olbrasoft.Data.Paging.Tests")]

namespace Olbrasoft.Data.Paging
{
    internal class PagedEnumerable<T> : List<T>, IPagedEnumerable<T>
    {
        private int _totalCount;

        public PagedEnumerable(IEnumerable<T> items)
        {
            AddRange(items);
        }

        public int TotalCount
        {
            get
            {
                if (_totalCount < Count)
                    _totalCount = Count;
                return _totalCount;
            }
            set
            {
                if (value < Count) throw new Exception($"{nameof(TotalCount)} cannot be less than the {nameof(Count)}");
                _totalCount = value;
            }
        }
    }
}