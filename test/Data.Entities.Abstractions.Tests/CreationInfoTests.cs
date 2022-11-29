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
}
