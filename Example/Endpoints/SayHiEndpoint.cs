namespace Example.Endpoints
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.Result;

    [RegistrateEndpoint]
    public class SayHiEndpoint : EndpointBase
    {
        public override string Route => @"/sayhi";

        public override HttpMethod Method => HttpMethod.Get;

        public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            var name = request.QueryParameters["name"];

            return await OkAsync(new { Message = $"Hello, dear {name}" });
        }
    }
}