namespace MultithredRest.Core.HttpServer;

using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

public class HttpRequest
{
    public string Route { get; init; } = string.Empty;

    public Encoding ContentEncoding { get; init; } = Encoding.Default;

    public long ContentLength64 { get; init; } = 0;

    public string UserHostAddress { get; init; } = string.Empty;

    public CookieCollection Cookies { get; init; } = [];

    public Dictionary<string, string> Headers { get; init; } = [];

    public HttpMethod HttpMethod { get; init; } = HttpMethod.Get;

    public Dictionary<string, string> QueryParameters { get; init; } = [];

    public byte[] BodyBytes { get; init; } = [];

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