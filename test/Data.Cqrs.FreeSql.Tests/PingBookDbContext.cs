using FreeSql;

namespace Data.Cqrs.FreeSql.Tests;

public class PingBookDbContext : DbContext
{
    public PingBookDbContext()
    {
    }


    public PingBookDbContext(IFreeSql fsql, DbContextOptions options) : base(fsql, options)
    {
    }


    protected override void OnModelCreating(ICodeFirst codefirst)
    {

        codefirst.Entity<PingAuthor>(eb =>
        {
            eb.ToTable("PingAuthor");
            eb.HasKey(e => e.Id);
            eb.Property(e => e.FirstName).IsRequired().HasMaxLength(255);
            eb.Property(e => e.LastName).IsRequired().HasMaxLength(255);

            eb.HasData(new[] {
                new PingAuthor { Id = 1, FirstName = "A", LastName = "B" },
                new PingAuthor { Id = 2, FirstName = "C", LastName = "D" },
                new PingAuthor { Id = 3, FirstName = "E", LastName = "F" }
            });
        });


        codefirst.Entity<PingBook>(eb =>
        {
            eb.ToTable("PingBook");
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Title).IsRequired().HasMaxLength(255);

            eb.HasData(new[] {
                new PingBook { Id = 1, Title = "Book1" },
                new PingBook { Id = 2, Title = "Book2" },
                new PingBook { Id = 3, Title = "A" }
            });
        });


        codefirst.SyncStructure<PingBook>();
        codefirst.SyncStructure<PingAuthor>();
        base.OnModelCreating(codefirst);
    }
}
