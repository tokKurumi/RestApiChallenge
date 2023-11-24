namespace MultithredRest.Helpers
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.EndpointModel;

    public static class AddEndpointsExtensions
    {
        public static void AddEndpoints(this IServiceCollection services)
        {
            var endpointBaseType = typeof(EndpointBase);

            var typesWithSingletonAttribute = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type =>
                    endpointBaseType.IsAssignableFrom(type) &&
                    type.GetCustomAttributes(typeof(RegistrateEndpointAttribute), true).Length > 0);

            foreach (var type in typesWithSingletonAttribute)
            {
                services.AddSingleton(endpointBaseType, type);
            }
        }
    }
}