namespace MultithredRest.Core
{
    public interface IEndpointsRoutes
    {
        IDictionary<string, EndpointBase> Instance { get; init; }
    }
}
