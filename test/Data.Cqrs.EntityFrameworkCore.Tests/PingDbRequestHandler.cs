using System.Linq.Expressions;

namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingDbRequestHandler : DbRequestHandler<PingLibraryDbContext, PingBook, PingBaseRequest, PingBook>
{

    public PingDbRequestHandler(PingLibraryDbContext context) : base(context)
    {
    }

    public PingDbRequestHandler(IProjector projector, PingLibraryDbContext context) : base(projector, context)
    {
    }

    public override Task<PingBook> HandleAsync(PingBaseRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }


    public new IQueryable<PingBook> Where(Expression<Func<PingBook, bool>> expression) => base.Where(expression);


    public new async Task<bool> ExistsAsync(Expression<Func<PingBook, bool>> predicate, CancellationToken token = default)
    => await base.AnyAsync(predicate, token);

    public new async Task<bool> ExistsAsync(CancellationToken token = default) => await base.AnyAsync(token);

    public new IOrderedQueryable<PingBook> GetOrderByDescending<TMember>(Expression<Func<PingBook, TMember>> columnSelector)
        => base.OrderByDescending(columnSelector);

    public new IOrderedQueryable<PingBook> GetOrderBy<TMember>(Expression<Func<PingBook, TMember>> columnSelector)
        => base.OrderBy(columnSelector);


    public new async Task<PingBook?> GetOneOrNullAsync(IQueryable<PingBook> queryable, CancellationToken token)
        => await base.GetOneOrNullAsync(queryable, token);


    public new async Task<PingBook?> GetOneOrNullAsync(Expression<Func<PingBook, bool>> expression, CancellationToken token)
 => await base.GetOneOrNullAsync(expression, token);


    public new async Task<IEnumerable<PingBook>> GetEnumerableAsync(IQueryable<PingBook> queryable, CancellationToken token)
        => await base.GetEnumerableAsync(queryable, token);


    public new async Task<IEnumerable<PingBook>> GetEnumerableAsync(Expression<Func<PingBook, bool>> exp, CancellationToken token)
    => await base.GetEnumerableAsync(exp, token);


    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<PingBook, bool>> expression, CancellationToken token = default)
      where TDestination : new()
       => await base.GetEnumerableAsync<TDestination>(expression, token);


    public new async Task<TDestination?> GetOneOrNullAsync<TDestination>(IQueryable<PingBook> queryable, CancellationToken token) where TDestination : new()
          => await base.GetOneOrNullAsync<TDestination>(queryable, token);

    public new async Task<TDestination?> GetOneOrNullAsync<TDestination>(Expression<Func<PingBook, bool>> expression, CancellationToken token)
    where TDestination : new()
         => await base.GetOneOrNullAsync<TDestination>(expression, token);


    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(IQueryable queryable, CancellationToken token)
   where TDestination : new()
   => await base.GetEnumerableAsync<TDestination>(queryable, token);


    public new async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
   where TDestination : new()
  => await base.GetEnumerableAsync<TDestination>(token);

}
