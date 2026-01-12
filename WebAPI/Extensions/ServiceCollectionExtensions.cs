using System.Reflection;

namespace WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var typesWithAttribute = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes<RegisterServiceAttribute>().Any())
            .ToList();

        foreach (var type in typesWithAttribute)
        {
            var attribute = type.GetCustomAttribute<RegisterServiceAttribute>();
            if (attribute != null)
            {
                services.Add(new ServiceDescriptor(type, type, attribute.Lifetime));
            }
        }
    }
}
