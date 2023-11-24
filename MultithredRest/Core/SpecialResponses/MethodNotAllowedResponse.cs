namespace MultithredRest.Core.SpecialResponses
{
    using System.Net;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;

    public class MethodNotAllowedResponse : SpecialResponseBase
    {
        public MethodNotAllowedResponse(HttpRequest request, EndpointBase endpoint)
        {
            RequestedMethod = request.HttpMethod;
            Endpoint = endpoint;

            StatusCode = HttpStatusCode.MethodNotAllowed;
        }

        public EndpointBase Endpoint { get; set; }

        public HttpMethod RequestedMethod { get; set; }
    }
}