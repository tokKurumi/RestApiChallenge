namespace MultithredRest.Core.RequestDispatcher
{
    using System.Net;
    using Microsoft.Extensions.Logging;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.SpecialResponses;
    using MultithredRest.Helpers;

    public class RequestDispatcher : IRequestDispatcher
    {
        private readonly ILogger<RequestDispatcher> _logger;
        private readonly IEndpointsRoutes _endpoints;

        public RequestDispatcher(ILogger<RequestDispatcher> logger, IEndpointsRoutes endpoints)
        {
            _logger = logger;
            _endpoints = endpoints;
        }

        public async Task<(ReadOnlyMemory<byte> Buffer, HttpStatusCode StatusCode, string ContentType)> DispatchAsync(HttpRequest request)
        {
            var route = request.Route;

            _logger.LogInformation("Dispatching connection from {IP} to {Route}", request.UserHostAddress, route);

            if (!_endpoints.Instance.TryGetValue(route, out var endpoint))
            {
                _logger.LogInformation("Dispatcher can not found given endpoint on the route {Route}", route);

                return (await new NotFoundResponse(request).SerializeJsonAsync(), HttpStatusCode.NotFound, @"application/json");
            }

            if (endpoint.Method != request.HttpMethod)
            {
                _logger.LogInformation("Dispatched connection to {Endpoint} with wrong method. Expected {ExpectedRequestMethod}, but given {GivenRequestMethod}", endpoint, endpoint.Method, request.HttpMethod);

                return (await new MethodNotAllowedResponse(request, endpoint).SerializeJsonAsync(), HttpStatusCode.MethodNotAllowed, @"application/json");
            }

            try
            {
                _logger.LogInformation("Dispatched connection to {Endpoint}", endpoint);
                return (await endpoint.GenerateResponseAsync(request), HttpStatusCode.OK, endpoint.HttpResponseContentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error in {Endpoint}", endpoint);
                return (await new InternalServerErrorResponse(request, endpoint, ex).SerializeJsonAsync(), HttpStatusCode.InternalServerError, @"application/json");
            }
        }
    }
}