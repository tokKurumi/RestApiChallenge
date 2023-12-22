namespace MultithredRest.Core.HttpServer
{
    public interface IHttpServer : IDisposable
    {
        string Protocol { get; set; }

        string Host { get; init; }

        bool IsWorking { get; }

        int Port { get; init; }

        Task StartAsync();

        void Stop();
    }
}