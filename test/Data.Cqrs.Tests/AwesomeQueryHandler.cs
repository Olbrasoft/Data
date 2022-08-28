namespace Olbrasoft.Data.Cqrs;

public class AwesomeQueryHandler : QueryHandler<Request<string>, string>
{
    public AwesomeQueryHandler(IProjector projector) : base(projector)
    {
    }

    public void CallProjectTo()
    {
        ProjectTo<string>(new List<char>().AsQueryable());
    }

    public override Task<string> HandleAsync(Request<string> query, CancellationToken token)
    {
        throw new System.NotImplementedException();
    }

    public void CallProjectToWithNullSource()
    {
        ProjectTo<string>(null);
    }
}