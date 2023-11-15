namespace MultithredRest.Core.Application
{
    using Microsoft.Extensions.Logging;
    using MultithredRest.Core.HttpServer;

    public class Application : IApplication
    {
        private readonly ILogger<Application> _logger;
        private readonly IHttpServer _server;

        public Application(ILogger<Application> logger, IHttpServer server)
        {
            _logger = logger;
            _server = server;
        }

        public void Run()
        {
            _logger.LogInformation("Application has succefully started");
            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
            _logger.LogInformation("Application has succefully stopped");
        }
    }
}