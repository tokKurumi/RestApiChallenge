namespace MultithredRest.Core
{
    using System.Net;
    using Microsoft.Extensions.Logging;
    using MultithreadRest.Helpers;

    public class RequestDispatcher : IRequestDispatcher
    {
        private readonly ILogger<RequestDispatcher> _logger;
        private readonly IEndpointsRoutes _endpoints;

        public RequestDispatcher(ILogger<RequestDispatcher> logger, IEndpointsRoutes endpoints)
        {
            _logger = logger;
            _endpoints = endpoints;
        }

        public Task Dispatch(HttpListenerContext context)
        {
            var route = context.Request.Url?.AbsolutePath!;

            _logger.LogInformation("Dispatching connection from {IP} to {Route}", context.Request.RemoteEndPoint, route);

            return Task.Run(async () =>
            {
                if (!_endpoints.Instance.ContainsKey(route))
                {
                    _logger.LogInformation("Dispatcher can not found given endpoint on the route {Route}", route);
                    return SendResponse(context.Response, await new { NotFound = "404" }.SerializeJsonAsync(), "application/json", HttpStatusCode.NotFound);
                }

                var endpoint = _endpoints.Instance[route];
                if (endpoint.Method.ToString() != context.Request.HttpMethod)
                {
                    _logger.LogInformation("Dispatched connection to {Route} with wrong method. Expected {ExpectedRequestMethod}, but given {GivenRequestMethod}", route, endpoint.Method, context.Request.HttpMethod);
                    return SendResponse(context.Response, await new { MethodNotAllowed = "405" }.SerializeJsonAsync(), "application/json", HttpStatusCode.MethodNotAllowed);
                }

                _logger.LogInformation("Dispatched connection to {Route}", route);
                return SendResponse(context.Response, await endpoint.GenerateResponse(context.Request), endpoint.HttpResponseContentType);
            });
        }

        private static HttpStatusCode SendResponse(HttpListenerResponse response, ReadOnlyMemory<byte> buffer, string contentType, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            response.ContentType = contentType;

            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer.Span);

            response.StatusCode = (int)statusCode;

            return statusCode;
        }
    }
}