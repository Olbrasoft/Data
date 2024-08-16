namespace Data.Cqrs.EntityFrameworkCore.Tests;

public class PingLibraryDbContext : DbContext
{
    public PingLibraryDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<PingBook> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<PingBook>().HasKey(e => e.Id);
        modelBuilder.Entity<PingBook>().Property(e => e.Title).IsRequired().HasMaxLength(255);

        modelBuilder.Entity<PingBook>().HasData(new[] {
            new PingBook { Id = 1, Title = "Book1" },
            new PingBook { Id = 2, Title = "Book2" },
            new PingBook { Id = 3, Title = "Book3" }
        });

        base.OnModelCreating(modelBuilder);

    }

}