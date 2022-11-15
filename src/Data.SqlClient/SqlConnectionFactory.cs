using Olbrasoft.Data.Common;
using System.Data.Common;
using System.Data.SqlClient;

namespace Olbrasoft.Data.SqlClient;

public class SqlConnectionFactory : DbConnectionFactory
{
    public SqlConnectionFactory(string connectionString) : base(connectionString)
    {
    }

    public override DbConnection CreateConnection()
    {     
        return new SqlConnection(ConnectionString);
    }
}