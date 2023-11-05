namespace MultithredRest.Services.AppServices
{
    public interface IHttpServer : IDisposable
    {
        string Host { get; init; }

        bool IsWorking { get; }

        int Port { get; init; }

        void Start();

        void Stop();
    }
}