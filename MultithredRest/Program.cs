using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultithredRest.Core;

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