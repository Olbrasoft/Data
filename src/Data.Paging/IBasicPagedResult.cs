using System.Collections.Generic;

namespace Olbrasoft.Data.Paging
{
    public interface IBasicPagedResult<T> : IList<T>
    {
        int TotalCount { get; }
    }
}