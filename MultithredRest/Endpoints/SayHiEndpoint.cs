namespace MultithredRest.Endpoints
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;

    public class SayHiEndpoint : EndpointBase
    {
        public override string Route => @"/sayhi";

        public override HttpMethod Method => HttpMethod.Get;

        public override string HttpResponseContentType => "application/json";

        public override async Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request)
        {
            try
            {
                var name = request.QueryParameters["name"];

                return await new { Message = $"Hello, dear {name}" }.SerializeJsonAsync();
            }
            catch (Exception ex)
            {
                return await new { Error = ex.Message }.SerializeJsonAsync();
            }
        }
    }
}