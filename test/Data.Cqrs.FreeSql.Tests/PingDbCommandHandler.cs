using FreeSql;
using Olbrasoft.Mapping;

namespace Data.Cqrs.FreeSql.Tests;
public class PingDbCommandHandler : DbCommandHandler<PingBookDbContext, PingBook, PingDbCommand, string>
{
    public PingDbCommandHandler(PingBookDbContext context) : base(context)
    {
    }

    public PingDbCommandHandler(IMapper mapper, PingBookDbContext context) : base(mapper, context)
    {
    }

    public new TDestination MapTo<TDestination>(object source) => base.MapTo<TDestination>(source);

    public new PingBook MapCommandToNewEntity(PingDbCommand command) => base.MapCommandToNewEntity(command);

    public new PingBook MapCommandToExistingEntity(PingDbCommand command, PingBook entity) => base.MapCommandToExistingEntity(command, entity);

    public static void CallThrowIfCommandIsNullOrCancellationRequested(PingDbCommand command, CancellationToken token) => ThrowIfCommandIsNullOrCancellationRequested(command, token);

    public new async Task<bool> SaveOneEntityAsync(CancellationToken token) => await base.SaveOneEntityAsync(token);

    public new DbSet<TForeignEntity> GetSet<TForeignEntity>() where TForeignEntity : class => base.GetSet<TForeignEntity>();

    public new DbSet<PingBook> Entities => base.Entities;

    //public new PingDbCommand? Command => base.Command;

    //public new void UseAutoChangeCommandStatus(PingDbCommand command) => base.UseAutoChangeCommandStatus(command);

    //public new bool TrySetCommandStatus(CommandStatus status) => base.TrySetCommandStatus(status);


    public override Task<string> HandleAsync(PingDbCommand request, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
