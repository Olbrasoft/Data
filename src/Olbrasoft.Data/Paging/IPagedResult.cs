namespace Olbrasoft.Data.Paging
{
    public interface IPagedResult<T> : IBasicPagedResult<T>
    {
        int FilteredCount { get; }
    }
}