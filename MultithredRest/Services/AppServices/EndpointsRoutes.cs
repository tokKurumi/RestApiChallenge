namespace MultithredRest.Services.AppServices
{
    using MultithredRest.Core;
    using MultithredRest.Endpoints;

    public class EndpointsRoutes : IEndpointsRoutes
    {
        public EndpointsRoutes()
        {
            Instance = new SortedDictionary<string, EndpointBase>(StringComparer.OrdinalIgnoreCase)
            {
                [@"/v1/helloworld"] = new HelloWorld(HttpMethod.Get),
                [@"/v1/weather"] = new Weather(HttpMethod.Post),
            };
        }

        public IDictionary<string, EndpointBase> Instance { get; init; }
    }
}