using System.Data.Common;

namespace Olbrasoft.Data.Common;

internal class AwesomeDbConnectionFactory : DbConnectionFactory
{
    public AwesomeDbConnectionFactory(string connectionString) : base(connectionString)
    {
    }

    public override DbConnection CreateConnection()
    {
        throw new NotImplementedException();
    }

    internal string GetConnectionString()
    {
        return ConnectionString;
    }
}