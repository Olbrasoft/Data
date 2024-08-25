using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Olbrasoft.Data.Cqrs;

public static class ServiceCollectionExtensions
{

    public static CqrsBuilder AddCqrs(this IServiceCollection services, params Assembly[] assemblies)
    {

        return services.AddCqrs(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

    }

    public static CqrsBuilder AddCqrs(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient, params Assembly[] assemblies)
    {

        return services.AddCqrs(cfg =>
        {

            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.Lifetime = lifetime;

        });

    }


    public static CqrsBuilder AddCqrs(this IServiceCollection services, Action<CqrsServiceConfiguration> configuration)
    {
        CqrsServiceConfiguration serviceConfig = new();

        configuration.Invoke(serviceConfig);

        return services.AddCqrs(serviceConfig);


    }


    public static CqrsBuilder AddCqrs(this IServiceCollection services, CqrsServiceConfiguration configuration)
    {

        if (configuration.AssembliesToRegister.Any())
        {
            MediationBuilder builder = services.AddMediation([.. configuration.AssembliesToRegister]);

            if (configuration.MediatorImplementationType == typeof(DynamicMediator))
            {
                builder.UseDynamicMediator(configuration.Lifetime);
            }
            else if (configuration.MediatorImplementationType == typeof(RequestHandlerMediator))
            {
                builder.UseRequestHandlerMediator(configuration.Lifetime);
            }
            else
            {
                throw new InvalidOperationException("Invalid mediator implementation type");
            }


            services.TryAdd(new ServiceDescriptor(typeof(ICommandExecutor), configuration.CommandExecutorImplementationType, configuration.Lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(IQueryProcessor), configuration.QueryProcessorImplementationType, configuration.Lifetime));

            return new CqrsBuilder(services);
        }

        throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");

    }

}