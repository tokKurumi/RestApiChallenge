# RestApiChallenge

RestApiChallenge is a custom framework for building simple REST API applications. This framework is not intended for production use; it is merely the result of a challenge to create a custom framework!

## Features:

1. **Easy to Use:** Designed for simplicity and ease of use.
2. **DI Support:** Built-in support for Dependency Injection.
3. **Content-Type Support:** Supports various Content-Types (application/json out of the box).
4. **Asynchronous Multithreading:** Asynchronous processing of requests for improved performance.
5. **Logging:** Integration of a simple and flexible logging mechanism to track actions and errors.
6. **Routing:** Powerful mechanism for defining and handling routes, making it easy to configure entry points for different requests.
7. **Extensibility:** Easy extension of framework functionality through additional modules or plugins.

## Getting Started:

### Prerequisites:
- .NET 8
- .NET Runtime

### Installation:
Clone the repository:
```bash
git clone https://github.com/tokKurumi/RestApiChallenge.git
```

## How to Use

1. Add the following code to your `Program.cs` file:
```csharp
using MultithreadRest.HostDefaults;

var host = MultithreadRestHost
    .CreateDefaultBuilder().Build();

host.RunConfigured();
```
2. Define your custom Endpoint in a new file:
```csharp
namespace Example.Endpoints;

using System.Net.Http;
using System.Threading.Tasks;
using MultithredRest.Core.Attributes;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;
using MultithredRest.Core.Result;

[RegistrateEndpoint]
public class HelloWorldEndpoint : EndpointBase
{
    public override HttpMethod Method => HttpMethod.Get;

    public override string Route => @"/helloworld";

    public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        return await OkAsync(new { Message = "Hello world!" });
    }
}
```
3. Everything is ready!

## Getting user provided parameters

To retrieve parameters in your endpoint, you can utilize various sources:
1. QueryParameters
2. Body
3. Cookies
4. Headers
```csharp
public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
{
    var queryParameters = request.QueryParameters;
    var body = await request.DeserializeBodyAsync<WeatherParam>(cancellationToken);
    var cookies = request.Cookies;
    var headers = request.Headers;
    var encoding = request.ContentEncoding;
    var contentLength = request.ContentLength64;

    return await OkAsync(await _weatherService.GetCityWeather(body.Postcode, body.CountryCode));
}
```

## Return Values

You can use the following methods to return values to the user:

1. OkAsync<T>(T @object)
2. OkAsync()
3. Ok(ReadOnlyMemory<byte> memory)
4. BadRequestAsync<T>(T @object)
5. NotFoundAsync<T>(T @object)
6. NotFoundAsync<T>(T @object)
7. InternalServerErrorAsync<T>(T @object)

## Dependency Injection Support

You can easily add dependency injection containers directly to your application! Here's an example of how to configure DI containers:

```csharp
using Example.Data;
using Example.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultithredRest.HostDefaults;

var host = MultithreadRestHost
    .CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IWeatherService, WeatherService>();
    })
    .Build();

host.RunConfigured();
```
```csharp
namespace Example.Endpoints
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Example.Models.WeatherApi;
    using Example.Services;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.Result;

    [RegistrateEndpoint]
    public class WeatherEndpoint : EndpointBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherEndpoint(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string Route => @"/weather";

        public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            var body = await request.DeserializeBodyAsync<WeatherParam>(cancellationToken);

            return await OkAsync(await _weatherService.GetCityWeather(body.Postcode, body.CountryCode));
        }
    }
}
```

## Dynamic Execution of Endpoints from Other Endpoints

```csharp
namespace Example.Endpoints;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MultithredRest.Core.Attributes;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;
using MultithredRest.Core.RequestDispatcher;
using MultithredRest.Core.Result;
using MultithredRest.Helpers;

[RegistrateEndpoint]
public class DynamicExecuteEndpoint : EndpointBase
{
    private readonly IServiceProvider _serviceProvider;

    public DynamicExecuteEndpoint(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override string Route => @"/dynamic";

    public override HttpMethod Method => HttpMethod.Post;

    protected IRequestDispatcher RequestDispatcher { get => _serviceProvider.GetRequiredService<IRequestDispatcher>(); }

    public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var parsedRequests = request.DeserializeEnumerableBodyAsync<HttpDynamicRequest>(cancellationToken);

        var results = new List<ReadOnlyMemory<byte>>();
        await foreach (var parsedRequest in parsedRequests)
        {
            if (parsedRequest is not null)
            {
                results.Add((await RequestDispatcher.DispatchAsync(parsedRequest.ToHttpRequest(request))).Buffer);

            }
        }

        return Ok(results.ConcatenateReadOnlyMemories(request.ContentEncoding.GetBytes("["), request.ContentEncoding.GetBytes(","), request.ContentEncoding.GetBytes("]")));
    }
}
```