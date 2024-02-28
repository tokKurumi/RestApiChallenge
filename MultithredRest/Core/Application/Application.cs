namespace MultithredRest.Core.Application;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MultithredRest.Core.HttpServer;

public class Application(ILogger<Application> logger, IHttpServer server)
    : BackgroundService
{
    private readonly ILogger<Application> _logger = logger;
    private readonly IHttpServer _server = server;

    public override void Dispose()
    {
        _server.Dispose();
        GC.SuppressFinalize(this);

        base.Dispose();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _server.Stop();
        _logger.LogInformation("Application has successfully stopped");

        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Application is launching");
        await _server.StartAsync(stoppingToken);
    }
}
