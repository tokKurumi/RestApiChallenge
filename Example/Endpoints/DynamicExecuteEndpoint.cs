namespace Example.Endpoints
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.RequestDispatcher;
    using MultithredRest.Core.Result;
    using MultithredRest.Helpers;

    [RegistrateEndpoint]
    public class DynamicExecuteEndpoint : EndpointBase
    {
        private readonly IServiceProvider _serviceProvider;

        public DynamicExecuteEndpoint(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override string Route => @"/dynamic";

        public override HttpMethod Method => HttpMethod.Post;

        protected IRequestDispatcher RequestDispatcher { get => _serviceProvider.GetRequiredService<IRequestDispatcher>(); }

        public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            var parsedRequests = request.DeserializeEnumerableBodyAsync<HttpDynamicRequest>(cancellationToken);

            var results = new List<ReadOnlyMemory<byte>>();
            await foreach (var parsedRequest in parsedRequests)
            {
                if (parsedRequest is not null)
                {
                    results.Add((await RequestDispatcher.DispatchAsync(parsedRequest.ToHttpRequest(request))).Buffer);

                }
            }

            return Ok(results.ConcatenateReadOnlyMemories(request.ContentEncoding.GetBytes("["), request.ContentEncoding.GetBytes(","), request.ContentEncoding.GetBytes("]")));
        }
    }
}