namespace Example.Endpoints;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MultithredRest.Core.Attributes;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;
using MultithredRest.Core.Result;

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