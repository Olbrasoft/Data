namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingLibraryDbContextTests
{
    //PingLibraryDbContext is public class
    [Fact]
    public void ShouldBePublicClass()
    {
        //Arrange
        var type = typeof(PingLibraryDbContext);

        //Act
        var result = type.IsPublic;

        //Assert
        Assert.True(result);
    }

    //PingLibraryDbContext inherit from DbContext
    [Fact]
    public void ShouldInheritFromDbContext()
    {
        //Arrange
        var type = typeof(PingLibraryDbContext);

        //Act
        var result = type.IsSubclassOf(typeof(DbContext));

        //Assert
        Assert.True(result);
    }

    //Create context with DbContextOptions in memmory database and add 3 books and result should be 3
    [Fact]
    public async Task ShouldAdd3Books()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(new Guid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();

        //Act
        context.Books.Add(new PingBook { Title = "Book1" });
        context.Books.Add(new PingBook { Title = "Book2" });
        context.Books.Add(new PingBook { Title = "Book3" });


        await context.SaveChangesAsync();

        var result = context.Books.Count();

        context.Database.EnsureDeleted();

        //Assert
        Assert.Equal(6, result);
    }


}
