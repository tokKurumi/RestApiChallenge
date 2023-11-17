namespace MultithredRest.Core.HttpServer
{
    using System.Net;
    using System.Text;
    using System.Text.Json;
    using MultithredRest.Helpers;

    public class HttpRequest
    {
        public HttpRequest(HttpDynamicRequest dynamicEndpointRequest, HttpRequest originalRequest)
        {
            Route = dynamicEndpointRequest.Route;
            ContentEncoding = Encoding.GetEncoding(dynamicEndpointRequest.ContentEncoding);
            ContentLength64 = dynamicEndpointRequest.Body.Length;
            UserHostAddress = originalRequest.UserHostAddress;
            Cookies = dynamicEndpointRequest.Cookies;
            Headers = dynamicEndpointRequest.Headers;
            HttpMethod = dynamicEndpointRequest.HttpMethod;
            QueryParameters = dynamicEndpointRequest.QueryParameters;

            BodyBytes = ReadBodyBytes(dynamicEndpointRequest.Body, ContentEncoding);
        }

        public HttpRequest(HttpListenerRequest request)
        {
            Route = request.Url?.AbsolutePath ?? string.Empty;
            ContentEncoding = request.ContentEncoding;
            ContentLength64 = request.ContentLength64;
            UserHostAddress = request.UserHostAddress;
            Cookies = request.Cookies;
            Headers = request.Headers.ToDictionary();
            HttpMethod = new HttpMethod(request.HttpMethod);
            QueryParameters = request.QueryString.ToDictionary();

            BodyBytes = ReadBodyBytes(request.InputStream, (int)ContentLength64);
        }

        public string Route { get; init; }

        public Encoding ContentEncoding { get; init; }

        public long ContentLength64 { get; init; }

        public string UserHostAddress { get; init; }

        public CookieCollection Cookies { get; init; }

        public Dictionary<string, string> Headers { get; init; }

        public HttpMethod HttpMethod { get; init; }

        public Dictionary<string, string> QueryParameters { get; init; }

        public byte[] BodyBytes { get; init; }

        public async Task<T> DeserializeBodyAsync<T>(CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream(BodyBytes);

            return await JsonSerializer.DeserializeAsync<T>(memoryStream, cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Deserialization failed.");
        }

        private static byte[] ReadBodyBytes(Stream bodyStream, int capacity)
        {
            using var memoryStream = new MemoryStream(capacity);
            bodyStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        private static byte[] ReadBodyBytes(string body, Encoding encoding)
        {
            return encoding.GetBytes(body);
        }
    }
}