﻿namespace MultithredRest.Endpoints
{
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.RequestDispatcher.RequestDispatcher;
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

        public override string HttpResponseContentType => "application/json";

        protected IRequestDispatcher RequestDispatcher { get => _serviceProvider.GetRequiredService<IRequestDispatcher>(); }

        public override async Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            using var file = new FileStream(@"Data/Dynamic.json", FileMode.Open);
            var parsedRequests = JsonSerializer.DeserializeAsyncEnumerable<HttpDynamicRequest>(file, cancellationToken: cancellationToken);

            var results = new List<ReadOnlyMemory<byte>>();
            await foreach (var parsedRequest in parsedRequests)
            {
                results.Add((await RequestDispatcher.DispatchAsync(parsedRequest?.ToHttpRequest(request) ?? throw new NullReferenceException("Null parsed"))).Buffer);
            }

            return results.ConcatenateReadOnlyMemories(request.ContentEncoding.GetBytes("["), request.ContentEncoding.GetBytes(","), request.ContentEncoding.GetBytes("]"));
        }
    }
}