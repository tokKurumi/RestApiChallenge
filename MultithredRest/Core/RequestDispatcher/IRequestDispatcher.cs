namespace MultithredRest.Core.RequestDispatcher.RequestDispatcher
{
    using System.Net;
    using MultithredRest.Core.HttpServer;

    public interface IRequestDispatcher
    {
        Task<(ReadOnlyMemory<byte> Buffer, HttpStatusCode StatusCode, string ContentType)> DispatchAsync(HttpRequest request);
    }
}