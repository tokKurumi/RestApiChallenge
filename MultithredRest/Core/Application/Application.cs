namespace MultithredRest.Core.Application;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MultithredRest.Core.HttpServer;
using System.Threading;
using System.Threading.Tasks;

public class Application : BackgroundService
{
    private readonly ILogger<Application> _logger;
    private readonly IHttpServer _server;

    public Application(ILogger<Application> logger, IHttpServer server)
    {
        _logger = logger;
        _server = server;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Application has succefully started");
        await _server.StartAsync();
    }
}