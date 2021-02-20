using System.Collections.Generic;

namespace Olbrasoft.Data.Paging
{
    internal class PagedResult<TRecord> : PagedEnumerable<TRecord>, IPagedResult<TRecord>
    {
        public PagedResult(IEnumerable<TRecord> items) : base(items)
        {
        }

        public int FilteredCount { get; set; }
    }
}