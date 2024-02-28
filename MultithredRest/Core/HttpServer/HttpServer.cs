namespace MultithredRest.Core.HttpServer;

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MultithredRest.Core.RequestDispatcher;
using MultithredRest.Helpers;

public class HttpServer(IRequestDispatcher dispatcher, ILogger<HttpServer> logger)
    : IHttpServer
{
    private readonly IRequestDispatcher _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    private readonly ILogger<HttpServer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly HttpListener _listener = new();
    private bool _isDisposed;

    public string Protocol { get; set; } = "http";

    public string Host { get; init; } = "localhost";

    public int Port { get; init; } = 8080;

    public async Task StartAsync(CancellationToken stoppingToken = default)
    {
        if (_listener.IsListening)
        {
            _logger.LogInformation("HttpServer is already running.");
            return;
        }

        _listener.Prefixes.Add($"{Protocol}://{Host}:{Port}/");
        _listener.Start();

        _logger.LogInformation("HttpServer has successfully started on {Protocol}://{Host}:{Port}/", Protocol, Host, Port);

        await StartConnectionHandlingAsync(stoppingToken);
    }

    public void Stop()
    {
        if (!_listener.IsListening)
        {
            _logger.LogInformation("HttpServer is not running.");
        }

        _listener.Stop();
        _logger.LogInformation("HttpServer has successfully stopped receiving new requests and terminated all outgoing requests");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            // free managed resources
            _listener.Close();
        }

        _isDisposed = true;
    }

    private async Task StartConnectionHandlingAsync(CancellationToken stoppingToken = default)
    {
        _logger.LogInformation("HttpServer has started handling incoming connections");

        while (_listener.IsListening && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                await Task.Run(() => HandleRequestAsync(context), stoppingToken);
            }
            catch (HttpListenerException)
            {
                // Handle exception when stopping the listener
                if (!_listener.IsListening && stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling request");
            }
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        try
        {
            var result = await _dispatcher.DispatchAsync(context.Request.ToHttpRequest()).ConfigureAwait(false);

            context.Response.ContentType = result.ContentType;
            context.Response.ContentLength64 = result.Buffer.Length;
            context.Response.StatusCode = (int)result.StatusCode;
            await context.Response.OutputStream.WriteAsync(result.Buffer).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling request");
        }
        finally
        {
            context.Response.OutputStream.Close();
        }
    }
}
