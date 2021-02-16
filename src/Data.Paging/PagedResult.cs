using System.Collections.Generic;

namespace Olbrasoft.Data.Paging
{
    public class PagedResult<TRecord> : BasicPagedResult<TRecord>, IPagedResult<TRecord>
    {
        public PagedResult(IEnumerable<TRecord> items) : base(items)
        {
        }

        public int FilteredCount { get; set; }
    }
}