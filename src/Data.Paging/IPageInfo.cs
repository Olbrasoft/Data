namespace Olbrasoft.Data.Paging
{
    public interface IPageInfo
    {
        int NumberOfSelectedPage { get; }
        int PageSize { get; }
    }
}