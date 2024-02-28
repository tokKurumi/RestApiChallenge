namespace MultithredRest.Core.HttpServer;

public interface IHttpServer : IDisposable
{
    string Protocol { get; set; }

    string Host { get; init; }

    int Port { get; init; }

    Task StartAsync(CancellationToken cancellationToken);

    void Stop();
}