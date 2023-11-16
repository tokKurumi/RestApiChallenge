namespace MultithredRest.Endpoints.HelloWorld.HelloWorld
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;

    public class HelloWorldEndpoint : EndpointBase
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string Route => @"/helloworld";

        public override string HttpResponseContentType => "application/json";

        public override async Task<ReadOnlyMemory<byte>> GenerateResponse(HttpRequestParameters requestParametres)
        {
            return await new { Message = "Hello world!" }.SerializeJsonAsync();
        }
    }
}