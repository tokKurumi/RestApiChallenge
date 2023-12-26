namespace Example.Endpoints
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.Result;

    [RegistrateEndpoint]
    public class HelloWorldEndpoint : EndpointBase
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string Route => @"/helloworld";

        public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            return await OkAsync(new { Message = "Hello world!" });
        }
    }
}