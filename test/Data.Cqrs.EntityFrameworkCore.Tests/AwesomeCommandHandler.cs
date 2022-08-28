namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

internal class AwesomeCommandHandler : DbCommandHandler<IRequest<int>, int, DbContext, AwesomeEntity>
{
    public AwesomeCommandHandler(IMapper mapper, DbContext context) : base(mapper, context)
    {
    }

    public override Task<int> HandleAsync(IRequest<int> command, CancellationToken token)
    {
        throw new System.NotImplementedException();
    }

    internal object GetProtectedPropertyContext()
    {
       return Context;
    }

    internal DbSet<AwesomeEntity> GetProtectedPropertyEntities()
    {
        return Entities;
    }
}