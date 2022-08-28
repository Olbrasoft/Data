namespace Olbrasoft.Data.Cqrs.Requests;

public class BooleanRequestByIdAndCreatorIdTest : BaseTest
{
    [Fact]
    public void BooleanRequestWithIdAndCreatorId_Inherit_From_BooleanRequestWithId()
    {
        //Arrange
        var type = typeof(ByIdRequest);

        //Act
        var request = new ByIdAndCreatorIdRequest(DispatcherMock.Object);

        //Assert
        Assert.IsAssignableFrom(type, request);
    }

    [Fact]
    public void BooleanRequestWithIdAndCreatorId_Has_A_CreatorId_Property_Of_Type_Integer()
    {
        //Arrange
        var request = new ByIdAndCreatorIdRequest(BooleanHandlerMock.Object);

        //Act
        var creatorId = request.CreatorId;

        //Assert
        Assert.IsAssignableFrom<int>(creatorId);
    }
}