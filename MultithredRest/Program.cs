using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultithredRest.Services.AppServices;
using MultithredRest.Services.ThirdParty;

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
                    .AddSingleton<IEndpointsRoutes, EndpointsRoutes>()
                    .AddSingleton<IWeatherApi, WeatherApi>();
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