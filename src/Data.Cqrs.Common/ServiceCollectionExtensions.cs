using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Olbrasoft.Data.Cqrs;

public static class ServiceCollectionExtensions
{

    public static CqrsBuilder AddCqrs(this IServiceCollection services, params Assembly[] assemblies)
    {

        if (assemblies != null && assemblies.Length != 0)
        {
            services.AddMediation(assemblies).UseDynamicMediator();

            services.TryAddTransient<ICommandExecutor, CommandExecutor>();

            services.TryAddTransient<IQueryProcessor, QueryProcessor>();

            return new CqrsBuilder(services);

        }

        throw new ArgumentNullException(nameof(assemblies));

    }

    public static CqrsBuilder AddCqrs(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient, params Assembly[] assemblies)
    {

        if (assemblies != null && assemblies.Length != 0)
        {
            services.AddMediation(assemblies).UseDynamicMediator(lifetime);

            services.TryAdd(new ServiceDescriptor(typeof(ICommandExecutor), typeof(CommandExecutor), lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(IQueryProcessor), typeof(QueryProcessor), lifetime));

            return new CqrsBuilder(services);

        }

        throw new ArgumentNullException(nameof(assemblies));

    }
}