namespace MultithredRest.Core.EndpointModel
{
    using System;
    using System.Net;
    using MultithredRest.Core.HttpServer;

    public abstract class EndpointBase
    {
        public EndpointBase(HttpMethod method)
        {
            Method = method;
        }

        public HttpMethod Method { get; init; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public string HttpResponseContentType { get; set; } = "application/json";

        public abstract Task<ReadOnlyMemory<byte>> GenerateResponse(HttpRequestParameters requestParametres);
    }
}