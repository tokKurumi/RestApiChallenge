namespace MultithredRest.Endpoints.Specials
{
    using System.Net;
    using System.Text;
    using MultithredRest.Core;
    using Newtonsoft.Json;

    public class NotFound : EndpointBase
    {
        public NotFound(HttpMethod method)
            : base(method)
        {
        }

        protected override ReadOnlySpan<byte> GenerateResponseBuffer(HttpListenerRequest request)
        {
            StatusCode = HttpStatusCode.NotFound;
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Error = "404" }, Formatting.Indented));
        }
    }
}