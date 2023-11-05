namespace MultithredRest.Endpoints.Specials
{
    using System.Net;
    using System.Text;
    using MultithredRest.Core;
    using Newtonsoft.Json;

    public class MethodNotAllowed : EndpointBase
    {
        public MethodNotAllowed(HttpMethod method)
            : base(method)
        {
        }

        protected override ReadOnlySpan<byte> GenerateResponseBuffer(HttpListenerRequest request)
        {
            StatusCode = HttpStatusCode.MethodNotAllowed;
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Error = $"Invalid method given, this method supports only {Method}" }, Formatting.Indented));
        }
    }
}