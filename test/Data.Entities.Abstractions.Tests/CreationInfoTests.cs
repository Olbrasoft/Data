namespace Olbrasoft.Data.Entities.Abstractions;
public class CreationInfoTests
{
    [Fact]
    public void Is_Abstract()
    {
        //Arrange
        var type = typeof(CreationInfo<object>);

        //Act
        var isAbstract = type.IsAbstract;

        //Assert
        Assert.True(isAbstract);
    }

    [Fact]
    public void CreationInfo_Implement_Interface_ICreatorInfo()
    {
        //Arrange
        var type = typeof(CreationInfo<object>);

        //Act
        var result = type.GetInterfaces().Where(p => p.Name.Contains("ICreatorInfo"));

        //Assert
        Assert.True(result.Count() == 1);
    }

    [Fact]
    public void Implement_Interface_IHaveCreated()
    {
        //Arrange
        var type = typeof(CreationInfo<object>);

        //Act
        var result = type.GetInterfaces().Where(p => p.Name == nameof(IHaveCreated));

        //Assert
        Assert.True(result.Count() == 1);
    }

    [Fact]
    public void AwesomeCreation_IsAssingable_From_CreationInfo()
    {
        //Arrange
        var type = typeof(CreationInfo<object>);

        //Act
        var creation = GetCreation();

        //Assert
        Assert.IsAssignableFrom(type, creation);
    }

    [Fact]
    public void AwesomeCreation_Id_Is_Integer()
    {
        //Arrange
        var creation = GetCreation();

        //Act
        var id = creation.Id;

        //Assert
        Assert.IsAssignableFrom<int>(id);
    }

    [Fact]
    public void AwesomeCreation_CreatorId_Is_Type_Of_Integer()
    {
        //Arrange
        var creation = GetCreation();

        //Act
        var creatorId = creation.CreatorId;

        //Assert
        Assert.IsAssignableFrom<int>(creatorId);
    }

    private static AwesomeCreation GetCreation()
    {
        return new AwesomeCreation();
    }
}
