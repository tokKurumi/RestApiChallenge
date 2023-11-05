namespace MultithredRest.Services.AppServices
{
    using Microsoft.Extensions.Logging;

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
            _server.Start();
            _logger.LogInformation("Application has succefully started");
        }

        public void Stop()
        {
            _server.Stop();
            _logger.LogInformation("Application has succefully stopped");
        }
    }
}