namespace Example.Endpoints;

using MultithredRest.Core.Attributes;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;
using MultithredRest.Core.Result;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[RegistrateEndpoint]
public class ExceptionEndpoint : EndpointBase
{
    public override string Route => @"/exception";

    public override HttpMethod Method => HttpMethod.Get;

    public override Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}