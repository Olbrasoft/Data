using System;
using System.Data.Common;

namespace Olbrasoft.Data.Common;

public abstract class DbConnectionFactory
{
	private readonly string _connectionString;
	protected string ConnectionString => _connectionString;

	protected DbConnectionFactory(string connectionString)
	{
		if (connectionString is null) throw new ArgumentNullException(nameof(connectionString));
		if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(connectionString));

		_connectionString = connectionString;
	}

	public abstract DbConnection CreateConnection();
}