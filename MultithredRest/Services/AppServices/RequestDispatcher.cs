namespace MultithredRest.Services.AppServices
{
    using System.Net;
    using Microsoft.Extensions.Logging;
    using MultithredRest.Endpoints.Specials;

    public class RequestDispatcher : IRequestDispatcher
    {
        private readonly ILogger<RequestDispatcher> _logger;
        private readonly IEndpointsRoutes _endpoints;

        public RequestDispatcher(ILogger<RequestDispatcher> logger, IEndpointsRoutes endpoints)
        {
            _logger = logger;
            _endpoints = endpoints;
        }

        public Task<HttpStatusCode> Dispatch(HttpListenerContext context)
        {
            var route = context.Request.Url?.AbsolutePath!;

            _logger.LogInformation("Dispatching connection from {IP} to {Route}", context.Request.RemoteEndPoint, route);

            return Task.Run(() =>
            {
                if (_endpoints.Instance.ContainsKey(route))
                {
                    var endpoint = _endpoints.Instance[route];

                    if (endpoint.Method.ToString() == context.Request.HttpMethod)
                    {
                        _logger.LogInformation("Dispatched connection to {Route}", route);
                        return _endpoints.Instance[route].Run(context);
                    }

                    _logger.LogInformation("Dispatched connection to {Route} with wrong method. Expected {ExpectedRequestMethod}, but given {GivenRequestMethod}", route, endpoint.Method, context.Request.HttpMethod);
                    return new MethodNotAllowed(HttpMethod.Get).Run(context);
                }

                _logger.LogInformation("Failed to dispatch connection, given unexisting route: {Endpoint}", route);
                return new NotFound(HttpMethod.Get).Run(context);
            });
        }
    }
}