using FreeSql;

namespace Data.Cqrs.FreeSql.Tests;

public class HaveCotextTests
{
    protected static readonly IFreeSql _freeSql = new FreeSqlBuilder()
    .UseConnectionString(DataType.Sqlite, "Data Source=:memory:;Cache=Shared;")
    .Build();

    private static readonly PingBookDbContext _context = new PingBookDbContext(_freeSql, new DbContextOptions());


    private PingBookDbContext CreateContext()
    {
        var options = new DbContextOptions();

        var context = new PingBookDbContext(_freeSql, options);

        return context;
    }



    protected PingBookDbContext Context
    {
        get
        {
            return _context;
        }
    }


}