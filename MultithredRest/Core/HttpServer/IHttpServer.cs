namespace MultithredRest.Core.HttpServer
{
    public interface IHttpServer : IDisposable
    {
        string Host { get; init; }

        bool IsWorking { get; }

        int Port { get; init; }

        Task StartAsync();

        void Stop();
    }
}