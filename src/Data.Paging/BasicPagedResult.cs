using System;
using System.Collections.Generic;

namespace Olbrasoft.Data.Paging
{
    public class BasicPagedResult<T> : List<T>, IBasicPagedResult<T>
    {
        private int _totalCount;

        public BasicPagedResult(IEnumerable<T> items)
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