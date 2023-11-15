namespace MultithredRest.Core.HttpServer
{
    using System.Net;
    using System.Text;
    using System.Text.Json;
    using MultithredRest.Helpers;

    public class HttpRequestParameters
    {
        public HttpRequestParameters(HttpListenerRequest request)
        {
            ContentEncoding = request.ContentEncoding;
            ContentLength64 = request.ContentLength64;
            UserHostAddress = request.UserHostAddress;
            Cookies = request.Cookies;
            Headers = request.Headers.ToDictionary();
            HttpMethod = new HttpMethod(request.HttpMethod);
            QueryParameters = request.QueryString.ToDictionary();

            BodyBytes = ReadBodyBytes(request.InputStream, (int)ContentLength64);
        }

        public Encoding ContentEncoding { get; init; }

        public long ContentLength64 { get; init; }

        public string UserHostAddress { get; init; }

        public CookieCollection Cookies { get; init; }

        public Dictionary<string, string> Headers { get; init; }

        public HttpMethod HttpMethod { get; init; }

        public Dictionary<string, string> QueryParameters { get; init; }

        protected byte[] BodyBytes { get; init; }

        public async Task<T?> DeserializeBodyAsync<T>(CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream(BodyBytes);

            return await JsonSerializer.DeserializeAsync<T>(memoryStream, cancellationToken: cancellationToken);
        }

        private static byte[] ReadBodyBytes(Stream bodyStream, int capacity)
        {
            using var memoryStream = new MemoryStream(capacity);
            bodyStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}