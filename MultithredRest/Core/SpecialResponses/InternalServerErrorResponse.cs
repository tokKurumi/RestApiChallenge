namespace MultithredRest.Core.SpecialResponses
{
    using System.Net;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;

    public class InternalServerErrorResponse : SpecialResponseBase
    {
        public InternalServerErrorResponse(HttpRequest request, EndpointBase endpoint, Exception exception)
        {
            Route = request.Route;
            Endpoint = endpoint;
            StackTrace = exception.StackTrace;

            StatusCode = HttpStatusCode.InternalServerError;
        }

        public string Route { get; set; }

        public EndpointBase Endpoint { get; set; }

        public string? StackTrace { get; set; }
    }
}