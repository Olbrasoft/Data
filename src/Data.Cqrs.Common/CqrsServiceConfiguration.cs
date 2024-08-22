using Microsoft.Extensions.DependencyInjection;
using Olbrasoft.Mediation;
using System.Reflection;

namespace Olbrasoft.Data.Cqrs;

public class CqrsServiceConfiguration
{

    /// <summary>
    /// Register various handlers from assembly containing given type
    /// </summary>
    /// <typeparam name="T">Type from assembly to scan</typeparam>
    /// <returns>This</returns>
    public CqrsServiceConfiguration RegisterServicesFromAssemblyContaining<T>()
        => RegisterServicesFromAssemblyContaining(typeof(T));

    /// <summary>
    /// Register various handlers from assembly containing given type
    /// </summary>
    /// <param name="type">Type from assembly to scan</param>
    /// <returns>This</returns>
    public CqrsServiceConfiguration RegisterServicesFromAssemblyContaining(Type type)
        => RegisterServicesFromAssembly(type.Assembly);

    /// <summary>
    /// Register various handlers from assembly
    /// </summary>
    /// <param name="assembly">Assembly to scan</param>
    /// <returns>This</returns>
    public CqrsServiceConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);

        return this;
    }

    /// <summary>
    /// Register various handlers from assemblies
    /// </summary>
    /// <param name="assemblies">Assemblies to scan</param>
    /// <returns>This</returns>
    public CqrsServiceConfiguration RegisterServicesFromAssemblies(
        params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);

        return this;
    }

    /// <summary>
    /// Gets or sets the list of assemblies to register.
    /// </summary>
    public List<Assembly> AssembliesToRegister { get; } = [];

    /// <summary>
    /// Gets or sets the service lifetime to register services under. Default value is <see cref="ServiceLifetime.Transient"/>.
    /// </summary>
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    /// Gets or sets the mediator implementation type to register. Default is <see cref="Mediator"/>.
    /// </summary>
    public Type MediatorImplementationType { get; set; } = typeof(DynamicMediator);

    /// <summary>
    /// Gets or sets the command executor implementation type. Default is <see cref="CommandExecutor"/>.
    /// </summary>
    public Type CommandExecutorImplementationType { get; set; } = typeof(CommandExecutor);

    /// <summary>
    /// Gets or sets the query processor implementation type. Default is <see cref="QueryProcessor"/>.
    /// </summary>
    public Type QueryProcessorImplementationType { get; set; } = typeof(QueryProcessor);
}
