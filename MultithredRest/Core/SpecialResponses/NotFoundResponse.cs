namespace MultithredRest.Core.SpecialResponses
{
    using System.Net;
    using MultithredRest.Core.HttpServer;

    public class NotFoundResponse : SpecialResponseBase
    {
        public NotFoundResponse(HttpRequest request)
        {
            Route = request.Route;

            StatusCode = HttpStatusCode.NotFound;
        }

        public string Route { get; set; }
    }
}