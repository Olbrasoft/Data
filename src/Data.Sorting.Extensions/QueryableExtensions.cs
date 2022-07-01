using Olbrasoft.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Data.Sorting;

namespace Olbrasoft.Extensions.Linq;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(
        this IQueryable<T> query,
        string orderByMember,
        OrderDirection direction = OrderDirection.Asc)
    {
        var queryElementTypeParam = Expression.Parameter(typeof(T));

        var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);

        var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

        var orderBy = Expression.Call(
            typeof(Queryable),
            direction == OrderDirection.Asc ? "OrderBy" : "OrderByDescending",
            new Type[] { typeof(T), memberAccess.Type },
            query.Expression,
            Expression.Quote(keySelector));

        return query.Provider.CreateQuery<T>(orderBy);
    }
}