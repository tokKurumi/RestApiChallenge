namespace MultithredRest.Core.HttpServer
{
    using System.Net;
    using Microsoft.Extensions.Logging;
    using MultithredRest.Core.RequestDispatcher;
    using MultithredRest.Helpers;

    public class HttpServer : IHttpServer
    {
        private readonly IRequestDispatcher _dispatcher;
        private readonly ILogger<HttpServer> _logger;
        private readonly HttpListener _listener = new HttpListener();
        private bool _disposing = false;

        public HttpServer(IRequestDispatcher dispatcher, ILogger<HttpServer> logger)
        {
            _dispatcher = dispatcher;
            _logger = logger;
        }

        public string Protocol { get; set; } = @"http";

        public string Host { get; init; } = @"localhost";

        public int Port { get; init; } = 8080;

        public bool IsWorking { get; private set; } = false;

        public async Task StartAsync()
        {
            _listener.Prefixes.Add(@$"{Protocol}://{Host}:{Port}/");
            _listener.Start();
            _logger.LogInformation("HttpServer has succefully started on {Host}:{Port}/", Host, Port);

            IsWorking = true;
            _logger.LogInformation("HttpServer working status changed to {WorkingStatus}", IsWorking);

            await StartConnectionHandlingAsync();
        }

        public void Stop()
        {
            _listener.Stop();
            _logger.LogInformation("HttpServer has succefully stoped");

            IsWorking = false;
            _logger.LogInformation("HttpServer working status changed to {WorkingStatus}", IsWorking);
        }

        public void Close()
        {
            if (IsWorking)
            {
                IsWorking = false;
                _logger.LogInformation("HttpServer working status changed to {WorkingStatus}", IsWorking);
            }

            _listener.Close();
            _logger.LogInformation("HttpServer has succefully closed connections");
        }

        public void Dispose() // dispose паттерн
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
                    Close();
                }
            }

            _disposing = true;
        }

        private async Task StartConnectionHandlingAsync()
        {
            _logger.LogInformation("HttpServer has started handeling incoming connections");

            while (IsWorking)
            {
                var context = await _listener.GetContextAsync();
                _ = Task.Run(() => HandleRequestAsync(context));
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
        }
    }
}