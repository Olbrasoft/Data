namespace Olbrasoft.Data.Paging
{
    public interface IPagedResult<T> : IPagedEnumerable<T>
    {
        int FilteredCount { get; }
    }
}