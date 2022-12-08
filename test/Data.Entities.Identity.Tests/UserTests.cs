using Microsoft.AspNetCore.Identity;
using Olbrasoft.Data.Entities.Abstractions;
using Xunit;

namespace Data.Entities.Identity.Tests;

public class UserTests
{

    [Fact]
    public void Instance_Inherit_From_IdentityUser_Of_Integer()
    {
        var type = typeof(IdentityUser<int>);

        var user = new AwesomeUser();

        Assert.IsAssignableFrom(type, user);
    }

    [Fact]
    public void Instance_Implement_interface_IHaveCreate()
    {
        var type = typeof(IHaveCreated);

        var user = new AwesomeUser();

        Assert.IsAssignableFrom(type, user);

        user.Created = DateTime.Now;
    }



    [Fact]
    public void Have_First_Name()
    {
        //Arrange
        var user = new AwesomeUser();

        //Act
        var name = user.FirstName;

        //Assert
        Assert.IsAssignableFrom<string>(name);
    }

    [Fact]
    public void Have_LastName()
    {
        //Arrange
        var user = new AwesomeUser();

        //Act
        var name = user.LastName;


        //Assert
        Assert.IsAssignableFrom<string>(name);

    }

    [Fact]
    public void Have_ToString()
    {
        //Arrange
        var user = new AwesomeUser { FirstName = "AwesomeFirstName", LastName = "AwesomeLastName" };

        //Act
        var result = user.ToString();

        //Assert
        Assert.True(result == "AwesomeFirstName AwesomeLastName");
    }


}