namespace MultithredRest.Endpoints
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using MultithreadRest.Helpers;
    using MultithredRest.Core;

    public class HelloWorld : EndpointBase
    {
        public HelloWorld(HttpMethod method)
            : base(method)
        {
        }

        public override async Task<ReadOnlyMemory<byte>> GenerateResponse(HttpListenerRequest request)
        {
            return await new { Message = "Hello world!" }.SerializeJsonAsync();
        }
    }
}