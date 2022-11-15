namespace Olbrasoft.Data.Cqrs.Exceptions;

public class QueryProcessorNullException : ArgumentNullException
{
    public QueryProcessorNullException() : base("processor")
    {

    }
}