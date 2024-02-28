namespace MultithredRest.Helpers;

using System.Net;
using System.Text;
using MultithredRest.Core.HttpServer;

public static class HttpRequestExtensions
{
    public static HttpRequest ToHttpRequest(this HttpDynamicRequest dynamicRequest, HttpRequest originalRequest)
    {
        return new HttpRequest()
        {
            Route = dynamicRequest.Route,
            ContentEncoding = Encoding.GetEncoding(dynamicRequest.ContentEncoding),
            ContentLength64 = dynamicRequest.Body.Length,
            UserHostAddress = originalRequest.UserHostAddress,
            Cookies = dynamicRequest.Cookies,
            Headers = dynamicRequest.Headers,
            HttpMethod = dynamicRequest.HttpMethod,
            QueryParameters = dynamicRequest.QueryParameters,
            BodyBytes = ReadBodyBytes(dynamicRequest.Body, Encoding.GetEncoding(dynamicRequest.ContentEncoding)),
        };
    }

    public static HttpRequest ToHttpRequest(this HttpListenerRequest request)
    {
        return new HttpRequest()
        {
            Route = request.Url?.AbsolutePath ?? string.Empty,
            ContentEncoding = request.ContentEncoding,
            ContentLength64 = request.ContentLength64,
            UserHostAddress = request.UserHostAddress,
            Cookies = request.Cookies,
            Headers = request.Headers.ToDictionary(),
            HttpMethod = new HttpMethod(request.HttpMethod),
            QueryParameters = request.QueryString.ToDictionary(),
            BodyBytes = ReadBodyBytes(request.InputStream, (int)request.ContentLength64),
        };
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