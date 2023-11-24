namespace MultithredRest.Core.HttpServer
{
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;

    public class HttpRequest
    {
        public HttpRequest()
        {
            Route = string.Empty;
            ContentEncoding = Encoding.Default;
            ContentLength64 = 0;
            UserHostAddress = string.Empty;
            Cookies = new CookieCollection();
            Headers = new Dictionary<string, string>();
            HttpMethod = HttpMethod.Get;
            QueryParameters = new Dictionary<string, string>();
            BodyBytes = Array.Empty<byte>();
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

        public async IAsyncEnumerable<T?> DeserializeEnumerableBodyAsync<T>([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var memoryStream = new MemoryStream(BodyBytes);

            await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<T>(memoryStream, cancellationToken: cancellationToken))
            {
                yield return item;
            }

            memoryStream.Dispose();
        }
    }
}