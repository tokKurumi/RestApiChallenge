namespace Example.Endpoints
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Helpers;

    [RegistrateEndpoint]
    public class HelloWorldEndpoint : EndpointBase
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string Route => @"/helloworld";

        public override string HttpResponseContentType => @"application/json";

        public override async Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            return await new { Message = "Hello world!" }.SerializeJsonAsync(cancellationToken);
        }
    }
}