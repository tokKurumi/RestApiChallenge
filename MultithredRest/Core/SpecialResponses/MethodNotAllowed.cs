namespace MultithredRest.Core.SpecialResponses;

using System.Net;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;

public class MethodNotAllowed : SpecialResponseBase
{
    public MethodNotAllowed(HttpRequest request, EndpointBase endpoint)
    {
        RequestedMethod = request.HttpMethod;
        Endpoint = endpoint;

        StatusCode = HttpStatusCode.MethodNotAllowed;
    }

    public EndpointBase Endpoint { get; set; }

    public HttpMethod RequestedMethod { get; set; }
}