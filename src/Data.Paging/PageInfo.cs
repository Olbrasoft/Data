namespace Olbrasoft.Data.Paging;

public class PageInfo : IPageInfo
{
    public int PageSize { get; }
    public int NumberOfSelectedPage { get; }

    public PageInfo(int pageSize = 10, int pageNumber = 1)
    {
        PageSize = pageSize;
        NumberOfSelectedPage = pageNumber;
    }
}