namespace MultithredRest.HostDefaults;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultithredRest.Core.Application;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;
using MultithredRest.Core.RequestDispatcher;

public static class MultithreadRestHost
{
    public static IHostBuilder CreateDefaultBuilder()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<IApplication, Application>()
                    .AddSingleton<IHttpServer, HttpServer>()
                    .AddSingleton<IRequestDispatcher, RequestDispatcher>()
                    .AddSingleton<IEndpointsRoutes, EndpointsRoutes>()
                    .AddEndpoints();
            });

        return host;
    }

    public static void RunConfigured(this IHost host)
    {
        host.Services
            .GetRequiredService<IApplication>()
            .Run();

        host.Run();
    }
}