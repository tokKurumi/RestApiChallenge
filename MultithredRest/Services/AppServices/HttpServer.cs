namespace MultithredRest.Services.AppServices
{
    using System.Net;
    using Microsoft.Extensions.Logging;

    public class HttpServer : IHttpServer
    {
        private readonly IRequestDispatcher _dispatcher;
        private readonly ILogger<HttpServer> _logger;
        private HttpListener _listener = new HttpListener();
        private bool _disposing = false;

        public HttpServer(IRequestDispatcher dispatcher, ILogger<HttpServer> logger)
        {
            _dispatcher = dispatcher;
            _logger = logger;
        }

        public string Host { get; init; } = @"http://localhost";

        public int Port { get; init; } = 8080;

        public bool IsWorking { get; private set; } = false;

        public void Start()
        {
            _listener.Prefixes.Add(@$"{Host}:{Port}/");
            _listener.Start();
            _logger.LogInformation("HttpServer has succefully started on {Host}:{Port}/", Host, Port);

            IsWorking = true;
            _logger.LogInformation("HttpServer working status changed to {WorkingStatus}", IsWorking);

            HandleIncomingConnections();
        }

        public void Stop()
        {
            _listener.Stop();
            _logger.LogInformation("HttpServer has succefully stoped");

            IsWorking = false;
            _logger.LogInformation("HttpServer working status changed to {WorkingStatus}", IsWorking);

            _listener.Close();
            _logger.LogInformation("HttpServer has succefully closed connections");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposing)
            {
                if (disposing)
                {
                    Stop();
                }
            }

            _disposing = true;
        }

        private void HandleIncomingConnections()
        {
            _logger.LogInformation("HttpServer has started handeling incoming connections");

            Task.Run(async () =>
            {
                while (IsWorking)
                {
                    var context = await _listener.GetContextAsync();
                    context.Response.ContentType = "application/json";

                    await _dispatcher.Dispatch(context);
                }
            });
        }
    }
}
