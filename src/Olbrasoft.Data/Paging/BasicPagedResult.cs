using System.Collections.Generic;

namespace Olbrasoft.Data.Paging
{
    public class BasicPagedResult<T> : List<T>, IBasicPagedResult<T>
    {
        public BasicPagedResult(IEnumerable<T> items)
        {
            AddRange(items);
        }

        public int TotalCount { get; set; }
    }
}