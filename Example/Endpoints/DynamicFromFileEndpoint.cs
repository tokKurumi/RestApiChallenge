namespace Example.Endpoints
{
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.RequestDispatcher;
    using MultithredRest.Core.Result;
    using MultithredRest.Helpers;

    [RegistrateEndpoint]
    public class DynamicFromFileEndpoint : EndpointBase
    {
        private IServiceProvider _serviceProvider;

        public DynamicFromFileEndpoint(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override string Route => @"/dynamicFromFile";

        public override HttpMethod Method => HttpMethod.Post;

        protected IRequestDispatcher RequestDispatcher { get => _serviceProvider.GetRequiredService<IRequestDispatcher>(); }

        public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            using var file = new FileStream(@"Data/Dynamic.json", FileMode.Open);
            var parsedRequests = JsonSerializer.DeserializeAsyncEnumerable<HttpDynamicRequest>(file, cancellationToken: cancellationToken);

            var results = new List<ReadOnlyMemory<byte>>();
            await foreach (var parsedRequest in parsedRequests)
            {
                results.Add((await RequestDispatcher.DispatchAsync(parsedRequest?.ToHttpRequest(request) ?? throw new NullReferenceException("Null parsed"))).Buffer);
            }

            return Ok(results.ConcatenateReadOnlyMemories(request.ContentEncoding.GetBytes("["), request.ContentEncoding.GetBytes(","), request.ContentEncoding.GetBytes("]")));
        }
    }
}