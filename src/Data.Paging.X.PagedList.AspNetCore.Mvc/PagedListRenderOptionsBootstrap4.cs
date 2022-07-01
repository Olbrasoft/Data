namespace Olbrasoft.Data.Paging.X.PagedList.AspNetCore.Mvc;

///<summary>
/// Options for configuring the output of <see cref = "HtmlHelper" />.
///</summary>
public partial class PagedListRenderOptions
{
    private const string _defaultContainerHtmlTag = "nav";

    private static readonly string[] _defaultUlElementClasses = { "pagination" };

    private static readonly string[] _defaultLiElementClasses = { "page-item" };

    private static readonly string[] _defaultAhrefElementClasses = { "page-link" };

    private const string _defaultLinkToPreviousPageFormat = "Previous";

    private const string _defaultLinkToNextPageFormat = "Next";

    private const string _defaultLinkToFirstPageFormat = "First";

    private const string _defaultLinkToLastPageFormat = "Last";

    private static void SetBootstrap4Option(PagedListRenderOptions option)
    {
        option.ContainerHtmlTag = _defaultContainerHtmlTag;
        option.UlElementClasses = _defaultUlElementClasses;
        option.LiElementClasses = _defaultLiElementClasses;
        option.AhrefElementClasses = _defaultAhrefElementClasses;
        option.LinkToPreviousPageFormat = _defaultLinkToPreviousPageFormat;
        option.LinkToNextPageFormat = _defaultLinkToNextPageFormat;
        option.LinkToFirstPageFormat = _defaultLinkToFirstPageFormat;
        option.LinkToLastPageFormat = _defaultLinkToLastPageFormat;
    }

    /// <summary>
    /// Only show Previous and Next links.
    /// </summary>
    public static PagedListRenderOptions Bootstrap4Minimal
    {
        get
        {
            var option = new PagedListRenderOptions();

            SetBootstrap4Option(option);
            SetMinimalOption(option);

            return option;
        }
    }

    /// <summary>
    /// Only show Page Numbers.
    /// </summary>
    public static PagedListRenderOptions Bootstrap4PageNumbersOnly
    {
        get
        {
            var option = new PagedListRenderOptions();

            SetBootstrap4Option(option);
            SetPageNumbersOnlyOption(option);

            return option;
        }
    }

    /// <summary>
    /// Show Page Numbers plus Previous and Next links.
    /// </summary>
    public static PagedListRenderOptions Bootstrap4PageNumbersPlusPrevAndNext
    {
        get
        {
            var option = new PagedListRenderOptions();

            SetBootstrap4Option(option);
            SetPageNumbersPlusPrevAndNextOption(option);

            return option;
        }
    }

    /// <summary>
    /// Show Page Numbers plus First and Last links.
    /// </summary>
    public static PagedListRenderOptions Bootstrap4PageNumbersPlusFirstAndLast
    {
        get
        {
            var option = new PagedListRenderOptions();

            SetBootstrap4Option(option);
            SetPageNumbersPlusFirstAndLastOption(option);

            return option;
        }
    }

    /// <summary>
    /// Show Page Numbers plus Previous, Next, First and Last links.
    /// </summary>
    public static PagedListRenderOptions Bootstrap4Full
    {
        get
        {
            var option = new PagedListRenderOptions();

            SetBootstrap4Option(option);
            SetFullOption(option);

            return option;
        }
    }
}