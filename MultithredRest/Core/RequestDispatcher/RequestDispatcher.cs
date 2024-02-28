namespace MultithredRest.Core.RequestDispatcher;

using Microsoft.Extensions.Logging;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;
using MultithredRest.Core.Result;
using MultithredRest.Core.SpecialResponses;

public class RequestDispatcher(ILogger<RequestDispatcher> logger, IEndpointsRoutes endpoints)
    : IRequestDispatcher
{
    private readonly ILogger<RequestDispatcher> _logger = logger;
    private readonly IEndpointsRoutes _endpoints = endpoints;

    public async Task<IActionResult> DispatchAsync(HttpRequest request)
    {
        var route = request.Route;

        _logger.LogInformation("Dispatching connection from {IP} to {Route}", request.UserHostAddress, route);

        if (!_endpoints.Instance.TryGetValue(route, out var endpoint))
        {
            _logger.LogInformation("Dispatcher can not found given endpoint on the route {Route}", route);

            return await ActionResult.InitializeAsync(new NotFound(request));
        }

        if (endpoint.Method != request.HttpMethod)
        {
            _logger.LogInformation("Dispatched connection to {Endpoint} with wrong method. Expected {ExpectedRequestMethod}, but given {GivenRequestMethod}", endpoint, endpoint.Method, request.HttpMethod);

            return await ActionResult.InitializeAsync(new MethodNotAllowed(request, endpoint));
        }

        try
        {
            _logger.LogInformation("Dispatched connection to {Endpoint}", endpoint);
            return await endpoint.GenerateResponseAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error in {Endpoint}", endpoint);

            return await ActionResult.InitializeAsync(new InternalServerError(request, endpoint, ex));
        }
    }
}