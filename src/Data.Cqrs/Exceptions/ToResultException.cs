namespace Olbrasoft.Data.Cqrs.Exceptions;

public class ToResultException : InvalidOperationException
{
    public ToResultException(string paramName) : base($"{paramName} and dispatcher is null.")
    {
    }
}