namespace MultithredRest.Core.Result;

using System.Net;
using MultithredRest.Core.SpecialResponses;
using MultithredRest.Helpers;

public class ActionResult : IActionResult
{
    public ActionResult(ReadOnlyMemory<byte> buffer, HttpStatusCode statusCode, string contentType)
    {
        Buffer = buffer;
        StatusCode = statusCode;
        ContentType = contentType;
    }

    private ActionResult()
    {
    }

    public ReadOnlyMemory<byte> Buffer { get; private set; } = default;

    public HttpStatusCode StatusCode { get; private set; } = default;

    public string ContentType { get; private set; } = string.Empty;

    public static async Task<IActionResult> InitializeAsync(SpecialResponseBase specialResponse)
    {
        var result = new ActionResult();
        await result.InitializeAsyncHelper(specialResponse);

        return result;
    }

    public static async Task<IActionResult> InitializeAsync<T>(T? @object, HttpStatusCode statusCode, string contentType)
    {
        var result = new ActionResult();
        await result.InitializeAsyncHelper(@object, statusCode, contentType);

        return result;
    }

    private async Task InitializeAsyncHelper(SpecialResponseBase specialResponse)
    {
        Buffer = specialResponse.GetType().Name switch
        {
            nameof(NotFound) => await ((NotFound)specialResponse).SerializeJsonAsync(),
            nameof(MethodNotAllowed) => await ((MethodNotAllowed)specialResponse).SerializeJsonAsync(),
            nameof(InternalServerError) => await ((InternalServerError)specialResponse).SerializeJsonAsync(),
            _ => await specialResponse.SerializeJsonAsync(),
        };

        StatusCode = specialResponse.StatusCode;
        ContentType = @"application/json";
    }

    private async Task InitializeAsyncHelper<T>(T? @object, HttpStatusCode statusCode, string contentType)
    {
        Buffer = @object is null ? ReadOnlyMemory<byte>.Empty : await @object.SerializeJsonAsync();
        StatusCode = statusCode;
        ContentType = contentType;
    }
}