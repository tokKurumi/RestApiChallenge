namespace MultithredRest.Core.HttpServer
{
    using System.Net;

    public class HttpDynamicRequest
    {
        public string Route { get; set; } = string.Empty;

        public string ContentEncoding { get; set; } = string.Empty;

        public CookieCollection Cookies { get; set; } = new CookieCollection();

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;

        public Dictionary<string, string> QueryParameters { get; set; } = new Dictionary<string, string>();

        public string Body { get; set; } = string.Empty;
    }
}