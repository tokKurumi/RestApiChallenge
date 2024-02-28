namespace MultithredRest.HostDefaults;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MultithredRest.Core.Attributes;
using MultithredRest.Core.Endpoint;

public static class AddEndpointsExtensions
{
    public static void AddEndpoints(this IServiceCollection services)
    {
        var endpointBaseType = typeof(EndpointBase);

        var typesWithSingletonAttribute = Assembly
            .GetEntryAssembly()
            ?.GetTypes()
            .Where(type =>
            {
                return
                    endpointBaseType.IsAssignableFrom(type)
                    && type.GetCustomAttributes(typeof(RegistrateEndpointAttribute), true).Length > 0;
            }) ?? [];

        foreach (var type in typesWithSingletonAttribute)
        {
            services.AddSingleton(endpointBaseType, type);
        }
    }
}