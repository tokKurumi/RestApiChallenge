namespace MultithredRest.Services.AppServices
{
    using MultithredRest.Core;

    public interface IEndpointsRoutes
    {
        IDictionary<string, EndpointBase> Instance { get; init; }
    }
}
