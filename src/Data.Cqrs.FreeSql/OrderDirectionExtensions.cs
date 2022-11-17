using Olbrasoft.Data.Sorting;

namespace Olbrasoft.Data.Cqrs.FreeSql;

public static class OrderDirectionExtensions
{
    /// <summary>
    /// Convert enum OrderDirection to Boolean;
    /// .Asc => True and .Desc => False
    /// </summary>
    /// <param name="direction">Sort direction</param>
    /// <returns>.Asc => True and .Desc => False</returns>
    public static bool ToBoolean(this OrderDirection direction)
    {
        return direction is OrderDirection.Asc;
    }
}