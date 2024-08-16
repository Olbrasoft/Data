using FreeSql;
using Moq;
using Olbrasoft.Mapping;

namespace Data.Cqrs.FreeSql.Tests;

public class PingBookDbRequestHandlerTests : HaveCotextTests
{



    //Constructor with parameter context  set property Context
    [Fact]
    public void Constructor_WithParameterContext_SetPropertyContext()
    {
        //Arrange
        var context = new PingBookDbContext();

        //Act
        var handler = new PingBookDbRequestHandler(context);

        //Assert
        handler.Context.Should().Be(context);
    }

    //Constructor with parameter mapper and context set property Mapper and Context
    [Fact]
    public void Constructor_WithParameterMapperAndContext_SetPropertyMapperAndContext()
    {
        //Arrange
        var mapper = new Mock<IMapper>().Object;
        var context = new PingBookDbContext();

        //Act
        var handler = new PingBookDbRequestHandler(mapper, context);

        //Assert
        handler.Mapper.Should().Be(mapper);
        handler.Context.Should().Be(context);
    }




    [Fact]
    public async Task ExistsAsync_ReturnTrue_UseConstructorWithParameterContext()
    {
        //Arrange
        var freeSql = _freeSql;

        var options = new DbContextOptions();


        var context = new PingBookDbContext(freeSql, options);



        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.ExistsAsync(default);
        var t = handler.GetWhere(x => x.Title == "Book2");
        var a = t.Count();


        //Assert
        result.Should().BeTrue();
    }





    //GetWhere with parameter condition return result count 1
    [Fact]
    public void GetWhere_WithParameterCondition_ReturnISelectPingBook()
    {

        var context = Context;

        ////Arrange
        var handler = new PingBookDbRequestHandler(context);


        ////Act
        var result = handler.GetWhere(x => x.Title == "Book2");


        ////Assert
        result.Count().Should().Be(1);

    }


    //GetOrderBy with parameter columnSelector c=>c.Title return first book id 3
    [Fact]
    public void GetOrderBy_WithParameterColumnSelector_ReturnISelectPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = handler.GetOrderBy(c => c.Title).First().Id;

        //Assert
        result.Should().Be(3);
    }

    //GetOneOrNullAsync with parameter condition return book with id 1
    [Fact]
    public async Task GetOneOrNullAsync_WithParameterCondition_ReturnPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetOneOrNullAsync(x => x.Id == 1, default);

        //Assert
        result.Id.Should().Be(1);
    }

    //GetEnumerableAsync with parameter condition return 2 books
    [Fact]
    public async Task GetEnumerableAsync_WithParameterCondition_ReturnPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync(x => x.Id > 1, default);

        //Assert
        result.Count().Should().Be(2);

    }


    //GetEnumerableAsync with parameter select return 2 books
    [Fact]
    public async Task GetEnumerableAsync_WithParameterSelect_ReturnPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync(handler.GetWhere(x => x.Id > 1), default);

        //Assert
        result.Count().Should().Be(2);
    }

    //GetOneOrNullAsync<PingBookDto> with parameter condition return book with id 1

    [Fact]
    public async Task GetOneOrNullAsync_WithParameterCondition_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetOneOrNullAsync<PingBookDto>(x => x.Id == 1, default);

        //Assert
        result.Title.Should().Be("Book1");
        result.Title2.Should().BeEmpty();
    }


    //GetEnumerableAsync<PingBookDto> with parameter condition return 2 books
    [Fact]
    public async Task GetEnumerableAsync_WithParameterCondition_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync<PingBookDto>(x => x.Id > 1, default);

        //Assert
        result.Count().Should().Be(2);
    }

    //GetEnumerableAsync<PingBookDto>  return 2 books
    [Fact]
    public async Task GetEnumerableAsync_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync<PingBookDto>(default);

        //Assert
        result.Count().Should().Be(3);
    }

    //GetEnumerableAsync<PingBookDto> with parameter select return 2 books
    [Fact]
    public async Task GetEnumerableAsync_WithParameterSelect_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync<PingBookDto>(handler.GetWhere(x => x.Id > 1), default);

        //Assert
        result.Count().Should().Be(2);
    }

    //GetOrderByDescending with parameter columnSelector c=>c.Title return first book id 2
    [Fact]
    public void GetOrderByDescending_WithParameterColumnSelector_ReturnISelectPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = handler.GetOrderByDescending(c => c.Title).First().Id;

        //Assert
        result.Should().Be(2);
    }

    //GetSelect<TForeignEntity> return ISelect<PingAuthor>
    [Fact]
    public void GetSelect_ReturnISelectPingAuthor()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = handler.GetSelect<PingAuthor>();

        //Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(3);
    }

    //Select return ISelect<PingBook>
    [Fact]
    public void Select_ReturnISelectPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = handler.Select;
        var select = handler.Select;

        //Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(3);
        select.Should().NotBeNull();
        select.Count().Should().Be(3);

    }


    //GetOneOrNullAsync with parameter select and mapTo return bookDto with Title2 Book1
    [Fact]
    public async Task GetOneOrNullAsync_WithParameterSelectAndMapTo_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetOneOrNullAsync(handler.GetWhere(x => x.Id == 1), x => new PingBookDto { Title2 = x.Title }, default);

        //Assert
        result.Title2.Should().Be("Book1");
        result.Title.Should().Be("Book1");

    }

    //GetEnumerableAsync with parameter mapTo return 3 books and first book Title2 is Book1
    [Fact]
    public async Task GetEnumerableAsync_WithParameterMapTo_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync(x => new PingBookDto { Title2 = x.Title }, default);

        //Assert
        result.Count().Should().Be(3);
        result.First().Title2.Should().Be("Book1");
    }

    //GetEnumerableAsync with parameter select and mapTo return 3 books and first book Title2 is Book1
    [Fact]
    public async Task GetEnumerableAsync_WithParameterSelectAndMapTo_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync(handler.GetWhere(x => x.Id > 0), x => new PingBookDto { Title2 = x.Title }, default);

        //Assert
        result.Count().Should().Be(3);
        result.First().Title2.Should().Be("Book1");
    }

    //GetEnumerableAsync with parameters condition id>1 and mapTo return 2 books and first book Title2 is Book2
    [Fact]
    public async Task GetEnumerableAsync_WithParametersConditionAndMapTo_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync(x => x.Id > 1, x => new PingBookDto { Title2 = x.Title }, default);

        //Assert
        result.Count().Should().Be(2);
        result.First().Title2.Should().Be("Book2");
    }

    //GetOneOrNullAsync<TDestination> with params condition id=2 and mapTo return bookDto with Title2 Book2
    [Fact]
    public async Task GetOneOrNullAsync_WithParametersConditionAndMapTo_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetOneOrNullAsync(x => x.Id == 2, x => new PingBookDto { Title2 = x.Title }, default);

        //Assert
        result.Title2.Should().Be("Book2");
    }


    //ExistsAsync with parameter exp return true
    [Fact]
    public async Task ExistsAsync_WithParameterExp_ReturnTrue()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.ExistsAsync(x => x.Id == 1, default);

        //Assert
        result.Should().BeTrue();
    }

    //ExistsAsync with parameter exp return false
    [Fact]
    public async Task ExistsAsync_WithParameterExp_ReturnFalse()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.ExistsAsync(x => x.Id == 0, default);

        //Assert
        result.Should().BeFalse();
    }


}
