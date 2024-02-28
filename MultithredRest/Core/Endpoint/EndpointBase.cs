namespace MultithredRest.Core.Endpoint;

using MultithredRest.Core.HttpServer;
using MultithredRest.Core.Result;

public abstract class EndpointBase
{
    public abstract string Route { get; }

    public abstract HttpMethod Method { get; }

    public static async Task<IActionResult> OkAsync<T>(T @object)
    {
        return await ActionResult.InitializeAsync(@object, System.Net.HttpStatusCode.OK, @"application/json");
    }

    public static IActionResult Ok(ReadOnlyMemory<byte> memory)
    {
        return new ActionResult(memory, System.Net.HttpStatusCode.OK, @"application/json");
    }

    public static async Task<IActionResult> OkAsync()
    {
        return await ActionResult.InitializeAsync<object>(null, System.Net.HttpStatusCode.OK, @"application/json");
    }

    public static async Task<IActionResult> BadRequestAsync<T>(T @object)
    {
        return await ActionResult.InitializeAsync(@object, System.Net.HttpStatusCode.BadRequest, @"application/json");
    }

    public static async Task<IActionResult> NotFoundAsync<T>(T @object)
    {
        return await ActionResult.InitializeAsync(@object, System.Net.HttpStatusCode.NotFound, @"application/json");
    }

    public static async Task<IActionResult> MethodNotAllowedAsync<T>(T @object)
    {
        return await ActionResult.InitializeAsync(@object, System.Net.HttpStatusCode.MethodNotAllowed, @"application/json");
    }

    public static async Task<IActionResult> InternalServerErrorAsync<T>(T @object)
    {
        return await ActionResult.InitializeAsync(@object, System.Net.HttpStatusCode.InternalServerError, @"application/json");
    }

    public abstract Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default);
}