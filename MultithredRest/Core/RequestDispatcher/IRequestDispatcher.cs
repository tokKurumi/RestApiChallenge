namespace MultithredRest.Core.RequestDispatcher;

using MultithredRest.Core.HttpServer;
using MultithredRest.Core.Result;

public interface IRequestDispatcher
{
    Task<IActionResult> DispatchAsync(HttpRequest request);
}