namespace MultithredRest.Core.Result;

using System.Net;

public interface IActionResult
{
    ReadOnlyMemory<byte> Buffer { get; }

    string ContentType { get; }

    HttpStatusCode StatusCode { get; }
}