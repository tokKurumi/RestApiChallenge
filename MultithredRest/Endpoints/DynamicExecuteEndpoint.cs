namespace MultithredRest.Endpoints
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.RequestDispatcher.RequestDispatcher;

    public class DynamicExecuteEndpoint : EndpointBase
    {
        private IServiceProvider _serviceProvider;

        public DynamicExecuteEndpoint(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override string Route => @"/dynamic";

        public override HttpMethod Method => HttpMethod.Post;

        public override string HttpResponseContentType => "application/json";

        protected IRequestDispatcher RequestDispatcher { get => _serviceProvider.GetRequiredService<IRequestDispatcher>(); }

        public override async Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request)
        {
            try
            {
                var parsedRequests = await request.DeserializeBodyAsync<List<HttpDynamicRequest>>();

                var results = new List<ReadOnlyMemory<byte>>();
                foreach (var parsedRequest in parsedRequests)
                {
                    results.Add((await RequestDispatcher.DispatchAsync(new HttpRequest(parsedRequest, request))).Buffer);
                }

                return ConcatenateReadOnlyMemories(results, request.ContentEncoding.GetBytes("["), request.ContentEncoding.GetBytes(","), request.ContentEncoding.GetBytes("]"));
            }
            catch (Exception ex)
            {
                return await new { Error = ex.Message }.SerializeJsonAsync();
            }
        }

        private static ReadOnlyMemory<byte> ConcatenateReadOnlyMemories(IEnumerable<ReadOnlyMemory<byte>> memories, ReadOnlyMemory<byte> open, ReadOnlyMemory<byte> separator, ReadOnlyMemory<byte> close)
        {
            int separatorLength = separator.Length;
            int openLength = open.Length;
            int closeLength = close.Length;

            // Calculate total length including separators and open/close sequences
            int totalLength = openLength + memories.Sum(memory => memory.Length + separatorLength) - separatorLength + closeLength;

            // Create the result array
            var resultArray = new byte[totalLength];
            int offset = 0;

            // Copy open sequence
            open.Span.CopyTo(resultArray.AsSpan(offset));
            offset += openLength;

            // Concatenate with separators
            foreach (var memory in memories)
            {
                if (offset > openLength)
                {
                    // Copy separator between memories
                    separator.Span.CopyTo(resultArray.AsSpan(offset));
                    offset += separatorLength;
                }

                // Copy memory
                memory.Span.CopyTo(resultArray.AsSpan(offset));
                offset += memory.Length;
            }

            // Copy close sequence
            close.Span.CopyTo(resultArray.AsSpan(offset));

            return resultArray.AsMemory();
        }
    }
}