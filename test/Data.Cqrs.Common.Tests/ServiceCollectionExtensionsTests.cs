namespace Data.Cqrs.Common.Tests;

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ServiceCollectionExtensions = Olbrasoft.Data.Cqrs.ServiceCollectionExtensions;

public class ServiceCollectionExtensionsTests
{
    //test ServiceCollectionExtensions is public static class ServiceCollectionExtensions
    [Fact]
    public void IsPublicStaticClass()
    {
        //Arrange
        Type type = typeof(ServiceCollectionExtensions);

        //Act
        var isPublic = type.IsPublic;
        var isStatic = type.IsAbstract && type.IsSealed;

        //Assert
        Assert.True(isPublic);
        Assert.True(isStatic);
    }

    //Assemly is Data.Cqrs.Common
    [Fact]
    public void AssemlyIsDataCqrsCommon()
    {
        //Arrange
        var type = typeof(ServiceCollectionExtensions);

        //Act
        var result = type.Assembly.GetName().Name;

        //Assert
        Assert.Equal("Olbrasoft.Data.Cqrs.Common", result);
    }


    //Namespace is Olbrasoft.Extensions.DependencyInjection
    [Fact]
    public void NamespaceIsOlbrasoftExtensionsDependencyInjection()
    {
        //Arrange
        var type = typeof(ServiceCollectionExtensions);

        //Act
        var result = type.Namespace;

        //Assert
        Assert.Equal("Olbrasoft.Data.Cqrs", result);
    }


    //AddCqrs return CqrsBuilder
    [Fact]
    public void AddCqrsReturnCqrsBuilder()
    {
        //Arrange
        var type = typeof(ServiceCollectionExtensions);

        var method = type.GetMethod("AddCqrs");

        //Act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var result = method.ReturnType;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        //Assert
        Assert.Equal(typeof(CqrsBuilder), result);
    }

    //AddCqrs register PingCommandHandler
    [Fact]
    public void AddCqrsRegisterPingCommandHandler()
    {
        //Arrange
        var services = new ServiceCollection();

        var assemblies = new[] { typeof(PingCommandHandler).Assembly };

        //Act
        services.AddCqrs(assemblies);


        var provider = services.BuildServiceProvider();


        var result = provider.GetRequiredService<IRequestHandler<PingCommand, object>>();

        //Assert
        Assert.IsType<PingCommandHandler>(result);
    }


    //AddCqrs throw ArgumentNullException when assemblies is null
    [Fact]
    public void AddCqrsThrowArgumentNullExceptionWhenAssembliesIsNull()
    {
        //Arrange
        var services = new ServiceCollection();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Assembly[] assemblies = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        //Act
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = Assert.Throws<ArgumentNullException>(() => services.AddCqrs(assemblies));
#pragma warning restore CS8604 // Possible null reference argument.

        //Assert
        Assert.Equal("assemblies", exception.ParamName);
    }

    //AddCqrs register ICommandExecutor
    [Fact]
    public void AddCqrsRegisterICommandExecutor()
    {
        //Arrange
        var services = new ServiceCollection();

        var assemblies = new[] { typeof(PingCommandHandler).Assembly };

        //Act
        services.AddCqrs(assemblies);

        var provider = services.BuildServiceProvider();

        var result = provider.GetRequiredService<ICommandExecutor>();

        //Assert
        Assert.IsType<CommandExecutor>(result);
    }

    //AddCqrs register IQueryProcessor
    [Fact]
    public void AddCqrsRegisterIQueryProcessor()
    {
        //Arrange
        var services = new ServiceCollection();

        var assemblies = new[] { typeof(Ping).Assembly };

        //Act
        services.AddCqrs(assemblies);

        var provider = services.BuildServiceProvider();

        var result = provider.GetRequiredService<IQueryProcessor>();

        //Assert
        Assert.IsType<QueryProcessor>(result);
    }


}

