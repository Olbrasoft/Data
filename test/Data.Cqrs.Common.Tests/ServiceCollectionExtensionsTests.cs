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



    //AddCqrs register PingCommandHandler
    [Fact]
    public void AddCqrsRegisterPingCommandHandler()
    {
        //Arrange
        var services = new ServiceCollection();

        var assemblies = new[] { typeof(PingCommandHandler).Assembly };

        //Act
        services.AddCqrs(ServiceLifetime.Transient, assemblies);


        var provider = services.BuildServiceProvider();


        var result = provider.GetRequiredService<IRequestHandler<PingCommand, object>>();

        //Assert
        Assert.IsType<PingCommandHandler>(result);
    }


 

    //AddCqrs register ICommandExecutor
    [Fact]
    public void AddCqrsRegisterICommandExecutor()
    {
        //Arrange
        var services = new ServiceCollection();

        var assemblies = new[] { typeof(PingCommandHandler).Assembly };

        //Act
        services.AddCqrs(ServiceLifetime.Transient,assemblies);

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
        services.AddCqrs(ServiceLifetime.Transient, assemblies);

        var provider = services.BuildServiceProvider();

        var result = provider.GetRequiredService<IQueryProcessor>();

        //Assert
        Assert.IsType<QueryProcessor>(result);
    }

    //AddCqrs with assemblies register PingCommandHandler
    [Fact]
    public void AddCqrsWithAssembliesRegisterPingCommandHandler()
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

    //AddCqrs with Action<CqrsServiceConfiguration> register RequestHandlerMediator
    [Fact]
    public void AddCqrsWithActionRegisterRequestHandlerMediator()
    {
        //Arrange
        var services = new ServiceCollection();

        //Act
        services.AddCqrs(cfg => { cfg.MediatorImplementationType = typeof(RequestHandlerMediator);
        cfg.RegisterServicesFromAssemblyContaining(typeof(Ping));   
        });

        var provider = services.BuildServiceProvider();

        var result = provider.GetRequiredService<IMediator>();

        //Assert
        Assert.IsType<RequestHandlerMediator>(result);
    }

    //AddCqrs with assemblies whe assemblies is empty throw ArgumentException
    [Fact]
    public void AddCqrsWithAssembliesWhenAssembliesIsEmptyThrowArgumentException()
    {
        //Arrange
        var services = new ServiceCollection();

        var assemblies = new Assembly[] { };

        //Act
        var exception = Assert.Throws<ArgumentException>(() => services.AddCqrs(assemblies));

        //Assert
        Assert.Equal("No assemblies found to scan. Supply at least one assembly to scan for handlers.", exception.Message);
    }

    //AddCqrs throw InvalidOperationException when unknown mediator implementation type
    [Fact]
    public void AddCqrsThrowInvalidOperationExceptionWhenUnknownMediatorImplementationType()
    {
        //Arrange
        var services = new ServiceCollection();

        //Act
        var exception = Assert.Throws<InvalidOperationException>(() => services.AddCqrs( cfg => 
            {
                cfg.MediatorImplementationType = typeof(object);
             cfg.RegisterServicesFromAssemblyContaining(typeof(Ping));
                       
            }));

        //Assert
        Assert.Equal("Unknown mediator implementation type.", exception.Message);
    }


}

