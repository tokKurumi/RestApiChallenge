namespace MultithredRest.Core.Endpoint
{
    public class EndpointsRoutes : IEndpointsRoutes
    {
        private readonly List<EndpointBase> _endpoints;

        public EndpointsRoutes(IEnumerable<EndpointBase> endpoints)
        {
            _endpoints = new List<EndpointBase>(endpoints);

            Instance = new SortedDictionary<string, EndpointBase>(StringComparer.OrdinalIgnoreCase);

            foreach (var endpoint in _endpoints)
            {
                Instance[endpoint.Route] = endpoint;
            }
        }

        public IDictionary<string, EndpointBase> Instance { get; init; }
    }
}