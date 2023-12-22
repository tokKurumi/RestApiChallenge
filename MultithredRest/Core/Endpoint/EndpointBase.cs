namespace MultithredRest.Core.Endpoint
{
    using System;
    using MultithredRest.Core.HttpServer;

    public abstract class EndpointBase
    {
        public abstract string Route { get; }

        public abstract HttpMethod Method { get; }

        public abstract string HttpResponseContentType { get; }

        public abstract Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default);
    }
}