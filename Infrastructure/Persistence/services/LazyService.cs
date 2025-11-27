using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.services
{
    public class LazyService<T>(IServiceProvider provider) : Lazy<T>(() => provider.GetRequiredService<T>()) where T : class
    {
    }
}
