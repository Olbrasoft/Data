namespace Olbrasoft.Data.Cqrs;

/// <summary>
/// Represents a base class for queries in the MediatR.Cqrs.Common namespace.
/// </summary>
public class BaseQuery<TResult> : BaseRequest<TResult>, IQuery<TResult>
{
    /// <summary>
    /// Gets the query processor.
    /// </summary>
    public IQueryProcessor? Processor { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseQuery{TResult}"/> class with the specified query processor.
    /// </summary>
    /// <param name="processor">The query processor.</param>
    public BaseQuery(IQueryProcessor processor)
    {
        Processor = processor is not null ? processor : throw new ArgumentNullException(nameof(processor));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseQuery{TResult}"/> class with the specified mediator.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public BaseQuery(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseQuery{TResult}"/> class.
    /// </summary>
    protected BaseQuery() { }
}
