using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultithredRest.Core.Application;
using MultithredRest.Core.EndpointModel;
using MultithredRest.Core.HttpServer;
using MultithredRest.Core.RequestDispatcher.RequestDispatcher;
using MultithredRest.Endpoints;
using MultithredRest.Services;

public class Program
{
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<IApplication, Application>()
                    .AddSingleton<IHttpServer, HttpServer>()
                    .AddSingleton<IRequestDispatcher, RequestDispatcher>()
                    .AddSingleton<IEndpointsRoutes, EndpointsRoutes>();

                services
                    .AddSingleton<EndpointBase, HelloWorldEndpoint>()
                    .AddSingleton<EndpointBase, WeatherEndpoint>()
                    .AddSingleton<EndpointBase, DynamicExecuteEndpoint>()
                    .AddSingleton<EndpointBase, SayHiEndpoint>();

                services
                    .AddSingleton<IWeatherService, WeatherService>();
            }).Build();

        var application = host.Services.GetRequiredService<IApplication>();

        var exitEvent = new ManualResetEvent(false);
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            exitEvent.Set();
        };

        application.Run();

        exitEvent.WaitOne();
        application.Stop();
    }
}