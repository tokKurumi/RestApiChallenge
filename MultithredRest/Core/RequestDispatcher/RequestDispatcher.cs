namespace MultithredRest.Core.RequestDispatcher.RequestDispatcher
{
    using System.Net;
    using Microsoft.Extensions.Logging;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;

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

            if (!_endpoints.Instance.ContainsKey(route))
            {
                _logger.LogInformation("Dispatcher can not found given endpoint on the route {Route}", route);

                return (await new { NotFound = "404" }.SerializeJsonAsync(), HttpStatusCode.NotFound, @"application/json");
            }

            var endpoint = _endpoints.Instance[route];
            if (endpoint.Method != request.HttpMethod)
            {
                _logger.LogInformation("Dispatched connection to {Route} with wrong method. Expected {ExpectedRequestMethod}, but given {GivenRequestMethod}", route, endpoint.Method, request.HttpMethod);

                return (await new { MethodNotAllowed = "405" }.SerializeJsonAsync(), HttpStatusCode.MethodNotAllowed, @"application/json");
            }

            _logger.LogInformation("Dispatched connection to {Route}", route);
            return (await endpoint.GenerateResponseAsync(request), HttpStatusCode.OK, @"application/json");
        }
    }
}