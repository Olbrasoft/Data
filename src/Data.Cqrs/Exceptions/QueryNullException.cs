namespace Olbrasoft.Data.Cqrs.Exceptions;

public class QueryNullException : ArgumentNullException
{
    public QueryNullException() : base("query")
    {
    }
}