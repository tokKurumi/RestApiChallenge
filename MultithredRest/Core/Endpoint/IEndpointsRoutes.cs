namespace MultithredRest.Core.EndpointModel
{
    public interface IEndpointsRoutes
    {
        IDictionary<string, EndpointBase> Instance { get; init; }
    }
}
