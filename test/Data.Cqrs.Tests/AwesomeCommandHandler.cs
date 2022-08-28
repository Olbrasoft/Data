namespace Olbrasoft.Data.Cqrs;

public class AwesomeCommandHandler<TCommand, TResult> : CommandHandler<TCommand, TResult> where TCommand : Request<TResult>
{
    public AwesomeCommandHandler(IMapper mapper) : base(mapper)
    {
    }

    public void Call_MapTo_Of_String_With_Parameter_Of_Type_String()
    {
        MapTo<string>("");
    }

    public override Task<TResult> HandleAsync(TCommand command, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}