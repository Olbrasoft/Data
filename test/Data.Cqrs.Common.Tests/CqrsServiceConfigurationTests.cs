using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Cqrs.Common.Tests;
public class CqrsServiceConfigurationTests
{
  
    [Fact]
    public void CqrsServiceConfiguration_WhenCreated_ShouldNotBeNull()
    {
        //Arrange
        CqrsServiceConfiguration configuration = new CqrsServiceConfiguration();
        //Act
        //Assert
        Assert.NotNull(configuration);
    }

    //CqrsServiceConfiguration is public class 
    [Fact]
    public void CqrsServiceConfiguration_is_public_class()
    {
        //Arrange
        var type = typeof(CqrsServiceConfiguration);

        //Act
        var result = type.IsPublic; 
        Assert.True(result);

    }

    //CqrsServiceConfiguration assembly is Data.Cqrs.Common
    [Fact]
    public void CqrsServiceConfiguration_assembly_is_Olbrasoft_Data_Cqrs_Common()
    {
        //Arrange
        var type = typeof(CqrsServiceConfiguration);

        //Act
        var result = type.Assembly.GetName().Name;
        Assert.Equal("Olbrasoft.Data.Cqrs.Common", result);

    }


    //namespace is Olbrasoft.Data.Cqrs
    [Fact]
    public void CqrsServiceConfiguration_namespace_is_Olbrasoft_Data_Cqrs()
    {
        //Arrange
        var type = typeof(CqrsServiceConfiguration);

        //Act
        var result = type.Namespace;
        Assert.Equal("Olbrasoft.Data.Cqrs", result);

    }

    //CqrsServiceConfiguration RegisterServicesFromAssemblyContaining<Ping> should be  AssembliesToRegister count 1

    [Fact]
    public void CqrsServiceConfiguration_RegisterServicesFromAssemblyContaining_Ping_should_be_AssembliesToRegister_count_1()
    {
        //Arrange
        CqrsServiceConfiguration configuration = new();
        
        //Act
         configuration.RegisterServicesFromAssemblyContaining<Ping>();
        
        //Assert
        Assert.Single(configuration.AssembliesToRegister);
    }

    //CqrsServiceConfiguration RegisterServicesFromAssemblies should be  AssembliesToRegister count 2
    [Fact]
    public void CqrsServiceConfiguration_RegisterServicesFromAssemblies_should_be_AssembliesToRegister_count_2()
    {
        //Arrange
        CqrsServiceConfiguration configuration = new();

        //Act
        configuration.RegisterServicesFromAssemblies(typeof(Ping).Assembly, typeof(IQueryProcessor).Assembly);

        //Assert
        Assert.Equal(2, configuration.AssembliesToRegister.Count);
    }


}
