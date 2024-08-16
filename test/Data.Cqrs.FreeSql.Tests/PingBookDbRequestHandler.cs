using FreeSql;
using Olbrasoft.Mapping;
using System.Linq.Expressions;

namespace Data.Cqrs.FreeSql.Tests;
public class PingBookDbRequestHandler : DbRequestHandler<PingBookDbContext, PingBook, PingBookRequest, string>
{
    public new PingBookDbContext Context => base.Context;
    public new IMapper? Mapper => base.Mapper;

    public new ISelect<PingBook> Select => base.Select;

    public new Task<bool> ExistsAsync(CancellationToken token) => base.ExistsAsync(token);



    public PingBookDbRequestHandler(PingBookDbContext context) : base(context)
    {
    }


    public PingBookDbRequestHandler(IMapper mapper, PingBookDbContext context) : base(mapper, context)
    {
    }


    public override Task<string> HandleAsync(PingBookRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public new ISelect<PingBook> GetWhere(Expression<Func<PingBook, bool>> condition) => base.GetWhere(condition);

    public new ISelect<PingBook> GetOrderBy<TMember>(Expression<Func<PingBook, TMember>> columnSelector) => base.GetOrderBy(columnSelector);

    public new async Task<PingBook> GetOneOrNullAsync(Expression<Func<PingBook, bool>> condition, CancellationToken token) => await base.GetOneOrNullAsync(condition, token);

    public new async Task<IEnumerable<PingBook>> GetEnumerableAsync(Expression<Func<PingBook, bool>> condition, CancellationToken token) => await base.GetEnumerableAsync(condition, token);

    public new async Task<IEnumerable<PingBook>> GetEnumerableAsync(ISelect<PingBook> select, CancellationToken token) => await base.GetEnumerableAsync(select, token);

    public new async Task<TDestination> GetOneOrNullAsync<TDestination>(Expression<Func<PingBook, bool>> condition, CancellationToken token) where TDestination : new()
        => await base.GetOneOrNullAsync<TDestination>(condition, token);

    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<PingBook, bool>> condition, CancellationToken token)
      where TDestination : new() => await base.GetEnumerableAsync<TDestination>(condition, token);

    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(ISelect<PingBook> select, CancellationToken token)
        where TDestination : new() => await base.GetEnumerableAsync<TDestination>(select, token);

    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
        where TDestination : new() => await base.GetEnumerableAsync<TDestination>(token);

    public new ISelect<PingBook> GetOrderByDescending<TMember>(Expression<Func<PingBook, TMember>> columnSelector) => base.GetOrderByDescending(columnSelector);

    public new ISelect<TForeignEntity> GetSelect<TForeignEntity>() where TForeignEntity : class => base.GetSelect<TForeignEntity>();

    public new Task<TDestination> GetOneOrNullAsync<TDestination>(ISelect<PingBook> select, Expression<Func<PingBook, TDestination>> mapTo, CancellationToken token) where TDestination : new()
        => base.GetOneOrNullAsync(select, mapTo, token);

    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<PingBook, TDestination>> mapTo, CancellationToken token)
       where TDestination : new() => await base.GetEnumerableAsync(mapTo, token);


    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(ISelect<PingBook> select, Expression<Func<PingBook, TDestination>> mapTo, CancellationToken token)
    where TDestination : new() => await base.GetEnumerableAsync(select, mapTo, token);


    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<PingBook, bool>> condition, Expression<Func<PingBook, TDestination>> mapTo, CancellationToken token)
        where TDestination : new() => await base.GetEnumerableAsync(condition, mapTo, token);


    public new Task<TDestination> GetOneOrNullAsync<TDestination>(Expression<Func<PingBook, bool>> condition, Expression<Func<PingBook, TDestination>> mapTo, CancellationToken token)
       where TDestination : new() => base.GetOneOrNullAsync(condition, mapTo, token);


    public new Task<bool> ExistsAsync(Expression<Func<PingBook, bool>> exp, CancellationToken token = default) => base.ExistsAsync(exp, token);

}