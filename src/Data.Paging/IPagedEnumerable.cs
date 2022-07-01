namespace Olbrasoft.Data.Paging;

public interface IPagedEnumerable<T> : IEnumerable<T>
{
    int TotalCount { get; }
}