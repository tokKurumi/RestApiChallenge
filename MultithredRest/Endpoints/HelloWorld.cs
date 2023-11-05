namespace MultithredRest.Endpoints
{
    using System;
    using System.Net;
    using System.Text;
    using MultithredRest.Core;
    using Newtonsoft.Json;

    public class HelloWorld : EndpointBase
    {
        public HelloWorld(HttpMethod method)
            : base(method)
        {
        }

        protected override ReadOnlySpan<byte> GenerateResponseBuffer(HttpListenerRequest request)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Message = "Hello world!" }, Formatting.Indented));
        }
    }
}