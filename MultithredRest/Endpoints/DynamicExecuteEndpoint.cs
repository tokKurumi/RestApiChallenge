namespace MultithredRest.Endpoints
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.RequestDispatcher.RequestDispatcher;
    using MultithredRest.Helpers;

    [RegistrateEndpoint]
    public class DynamicExecuteEndpoint : EndpointBase
    {
        private IServiceProvider _serviceProvider;

        public DynamicExecuteEndpoint(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override string Route => @"/dynamic";

        public override HttpMethod Method => HttpMethod.Post;

        public override string HttpResponseContentType => "application/json";

        protected IRequestDispatcher RequestDispatcher { get => _serviceProvider.GetRequiredService<IRequestDispatcher>(); }

        public override async Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            var parsedRequests = request.DeserializeEnumerableBodyAsync<HttpDynamicRequest>(cancellationToken);

            var results = new List<ReadOnlyMemory<byte>>();
            await foreach (var parsedRequest in parsedRequests)
            {
                results.Add((await RequestDispatcher.DispatchAsync(parsedRequest.ToHttpRequest(request))).Buffer);
            }

            return results.ConcatenateReadOnlyMemories(request.ContentEncoding.GetBytes("["), request.ContentEncoding.GetBytes(","), request.ContentEncoding.GetBytes("]"));
        }
    }
}