using Olbrasoft.Data.Paging;
using System;

// ReSharper disable once CheckNamespace
namespace Olbrasoft.Extensions.Paging
{
    public static class PageInfoExtensions
    {
        public static int CalculateSkip(this IPageInfo pageInfo)
        {
            if (pageInfo is null) throw new ArgumentNullException(nameof(pageInfo));

            return (pageInfo.NumberOfSelectedPage - 1) * pageInfo.PageSize;
        }
    }
};