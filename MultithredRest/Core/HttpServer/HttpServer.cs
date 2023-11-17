﻿namespace MultithredRest.Core.HttpServer
{
    using System.Net;
    using Microsoft.Extensions.Logging;
    using MultithredRest.Core.RequestDispatcher.RequestDispatcher;

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

            StartConnectionHandling();
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
                    Close();
                }
            }

            _disposing = true;
        }

        private void StartConnectionHandling()
        {
            Task.Run(HandleIncomingConnectionsAsync);
        }

        private async Task HandleIncomingConnectionsAsync()
        {
            _logger.LogInformation("HttpServer has started handeling incoming connections");

            while (IsWorking)
            {
                var context = await _listener.GetContextAsync();

                var result = await _dispatcher.DispatchAsync(new HttpRequest(context.Request));

                context.Response.ContentType = result.ContentType;
                context.Response.ContentLength64 = result.Buffer.Length;
                context.Response.StatusCode = (int)result.StatusCode;
                await context.Response.OutputStream.WriteAsync(result.Buffer);
            }
        }
    }
}
