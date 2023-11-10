namespace MultithredRest.Core
{
    using MultithredRest.Endpoints;

    public class EndpointsRoutes : IEndpointsRoutes
    {
        public EndpointsRoutes()
        {
            Instance = new SortedDictionary<string, EndpointBase>(StringComparer.OrdinalIgnoreCase)
            {
                [@"/v1/helloworld"] = new HelloWorld(HttpMethod.Get),
            };
        }

        public IDictionary<string, EndpointBase> Instance { get; init; }
    }
}