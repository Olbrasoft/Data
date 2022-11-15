namespace Olbrasoft.Data.Cqrs.Exceptions;

public class CommandExecutorNullException : ArgumentNullException
{
	public CommandExecutorNullException() : base("executor")
	{

	}
}