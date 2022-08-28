namespace Olbrasoft.Data.Cqrs;

public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>
{
    private readonly IMapper _mapper;

    protected CommandHandler(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public abstract Task<TResult> HandleAsync(TCommand command, CancellationToken token = default);

    protected TDestination MapTo<TDestination>(object source) => _mapper.MapTo<TDestination>(source);
}