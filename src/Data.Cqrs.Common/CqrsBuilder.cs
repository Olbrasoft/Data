using Microsoft.Extensions.DependencyInjection;

namespace Olbrasoft.Data.Cqrs;

public class CqrsBuilder(IServiceCollection services)
{
    public IServiceCollection Services { get; init; } = services ?? throw new ArgumentNullException(nameof(services));
}