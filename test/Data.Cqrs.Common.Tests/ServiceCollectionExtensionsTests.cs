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
        bool isPublic = type.IsPublic;
        bool isStatic = type.IsAbstract && type.IsSealed;

        //Assert
        Assert.True(isPublic);
        Assert.True(isStatic);
    }

    //Assemly is Data.Cqrs.Common
    [Fact]
    public void AssemlyIsDataCqrsCommon()
    {
        //Arrange
        Type type = typeof(ServiceCollectionExtensions);

        //Act
        string? result = type.Assembly.GetName().Name;

        //Assert
        Assert.Equal("Olbrasoft.Data.Cqrs.Common", result);
    }


    //Namespace is Olbrasoft.Extensions.DependencyInjection
    [Fact]
    public void NamespaceIsOlbrasoftExtensionsDependencyInjection()
    {
        //Arrange
        Type type = typeof(ServiceCollectionExtensions);

        //Act
        string? result = type.Namespace;

        //Assert
        Assert.Equal("Olbrasoft.Data.Cqrs", result);
    }



    //AddCqrs register PingCommandHandler
    [Fact]
    public void AddCqrsRegisterPingCommandHandler()
    {
        //Arrange
        ServiceCollection services = new();

        Assembly[] assemblies = new[] { typeof(PingCommandHandler).Assembly };

        //Act
        services.AddCqrs(ServiceLifetime.Transient, assemblies);


        ServiceProvider provider = services.BuildServiceProvider();


        IRequestHandler<PingCommand, object> result = provider.GetRequiredService<IRequestHandler<PingCommand, object>>();

        //Assert
        Assert.IsType<PingCommandHandler>(result);
    }




    //AddCqrs register ICommandExecutor
    [Fact]
    public void AddCqrsRegisterICommandExecutor()
    {
        //Arrange
        ServiceCollection services = new();

        Assembly[] assemblies = new[] { typeof(PingCommandHandler).Assembly };

        //Act
        services.AddCqrs(ServiceLifetime.Transient, assemblies);

        ServiceProvider provider = services.BuildServiceProvider();

        ICommandExecutor result = provider.GetRequiredService<ICommandExecutor>();

        //Assert
        Assert.IsType<CommandExecutor>(result);
    }

    //AddCqrs register IQueryProcessor
    [Fact]
    public void AddCqrsRegisterIQueryProcessor()
    {
        //Arrange
        ServiceCollection services = new();

        Assembly[] assemblies = new[] { typeof(Ping).Assembly };

        //Act
        services.AddCqrs(ServiceLifetime.Transient, assemblies);

        ServiceProvider provider = services.BuildServiceProvider();

        IQueryProcessor result = provider.GetRequiredService<IQueryProcessor>();

        //Assert
        Assert.IsType<QueryProcessor>(result);
    }

    //AddCqrs with assemblies register PingCommandHandler
    [Fact]
    public void AddCqrsWithAssembliesRegisterPingCommandHandler()
    {
        //Arrange
        ServiceCollection services = new();

        Assembly[] assemblies = new[] { typeof(PingCommandHandler).Assembly };

        //Act
        services.AddCqrs(assemblies);

        ServiceProvider provider = services.BuildServiceProvider();

        IRequestHandler<PingCommand, object> result = provider.GetRequiredService<IRequestHandler<PingCommand, object>>();

        //Assert
        Assert.IsType<PingCommandHandler>(result);
    }

    //AddCqrs with Action<CqrsServiceConfiguration> register RequestHandlerMediator
    [Fact]
    public void AddCqrsWithActionRegisterRequestHandlerMediator()
    {
        //Arrange
        ServiceCollection services = new();

        //Act
        services.AddCqrs(cfg =>
        {
            cfg.MediatorImplementationType = typeof(RequestHandlerMediator);
            cfg.RegisterServicesFromAssemblyContaining(typeof(Ping));
        });

        ServiceProvider provider = services.BuildServiceProvider();

        IMediator result = provider.GetRequiredService<IMediator>();

        //Assert
        Assert.IsType<RequestHandlerMediator>(result);
    }

    //AddCqrs with assemblies whe assemblies is empty throw ArgumentException
    [Fact]
    public void AddCqrsWithAssembliesWhenAssembliesIsEmptyThrowArgumentException()
    {
        //Arrange
        ServiceCollection services = new();

        Assembly[] assemblies = new Assembly[] { };

        //Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => services.AddCqrs(assemblies));

        //Assert
        Assert.Equal("No assemblies found to scan. Supply at least one assembly to scan for handlers.", exception.Message);
    }

    //AddCqrs throw InvalidOperationException when unknown mediator implementation type
    [Fact]
    public void AddCqrsThrowInvalidOperationExceptionWhenUnknownMediatorImplementationType()
    {
        //Arrange
        ServiceCollection services = new();

        //Act
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => services.AddCqrs(cfg =>
        {
            cfg.MediatorImplementationType = typeof(object);
            cfg.RegisterServicesFromAssemblyContaining(typeof(Ping));
        }));

        //Assert
        Assert.Equal("Invalid mediator implementation type", exception.Message);
    }


}

