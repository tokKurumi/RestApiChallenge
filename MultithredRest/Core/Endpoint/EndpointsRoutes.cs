namespace MultithredRest.Core.EndpointModel
{
    using Microsoft.Extensions.DependencyInjection;
    using MultithredRest.Endpoints.HelloWorld.HelloWorld;
    using MultithredRest.Endpoints.Weather;

    public class EndpointsRoutes : IEndpointsRoutes
    {
        private IServiceProvider _serviceProvider;

        public EndpointsRoutes(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Instance = new SortedDictionary<string, EndpointBase>(StringComparer.OrdinalIgnoreCase)
            {
                [@"/helloworld"] = _serviceProvider.GetRequiredService<IHelloWorld>() as EndpointBase,
                [@"/weather"] = _serviceProvider.GetRequiredService<IWeather>() as EndpointBase,
            };
        }

        public IDictionary<string, EndpointBase> Instance { get; init; }
    }
}