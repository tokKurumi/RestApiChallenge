namespace MultithredRest.Endpoints
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;

    [RegistrateEndpoint]
    public class SayHiEndpoint : EndpointBase
    {
        public override string Route => @"/sayhi";

        public override HttpMethod Method => HttpMethod.Get;

        public override string HttpResponseContentType => "application/json";

        public override async Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            var name = request.QueryParameters["name"];

            return await new { Message = $"Hello, dear {name}" }.SerializeJsonAsync(cancellationToken);
        }
    }
}