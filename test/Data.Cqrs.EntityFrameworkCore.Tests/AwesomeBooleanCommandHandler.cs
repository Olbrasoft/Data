namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

internal class AwesomeBooleanCommandHandler : DbCommandHandler<IRequest<bool>, DbContext, AwesomeEntity>
{
    public AwesomeBooleanCommandHandler(IMapper mapper, DbContext context) : base(mapper, context)
    {
    }

    public override Task<bool> HandleAsync(IRequest<bool> command, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}