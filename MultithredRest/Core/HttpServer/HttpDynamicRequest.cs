namespace MultithredRest.Core.HttpServer
{
    using System.Net;

    public class HttpDynamicRequest
    {
        public HttpDynamicRequest()
        {
            Route = string.Empty;
            ContentEncoding = string.Empty;
            Cookies = new CookieCollection();
            Headers = new Dictionary<string, string>();
            HttpMethod = HttpMethod.Get;
            QueryParameters = new Dictionary<string, string>();
            Body = string.Empty;
        }

        public string Route { get; set; }

        public string ContentEncoding { get; set; }

        public CookieCollection Cookies { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public Dictionary<string, string> QueryParameters { get; set; }

        public string Body { get; set; }
    }
}