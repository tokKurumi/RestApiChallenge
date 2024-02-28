namespace MultithredRest.Core.Endpoint;

public interface IEndpointsRoutes
{
    IDictionary<string, EndpointBase> Instance { get; init; }
}
