namespace Olbrasoft.Data.Cqrs.Exceptions;

public class CommandNullException : ArgumentNullException
{
	public CommandNullException() : base("command")
	{

	}

}