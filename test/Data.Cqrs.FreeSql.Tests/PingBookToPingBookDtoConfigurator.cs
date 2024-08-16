using Olbrasoft.Data.Cqrs.FreeSql;

namespace Data.Cqrs.FreeSql.Tests;
public class PingBookToPingBookDtoConfigurator : IConfigure<PingBook>
{
    public System.Linq.Expressions.Expression<Func<PingBook, TDestination>> Configure<TDestination>() where TDestination : new()
    {
        return pingBook => new TDestination
        {

        };
    }
}
