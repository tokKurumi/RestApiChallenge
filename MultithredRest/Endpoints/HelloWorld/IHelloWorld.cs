namespace MultithredRest.Endpoints.HelloWorld.HelloWorld
{
    using MultithredRest.Core.HttpServer;

    public interface IHelloWorld
    {
        Task<ReadOnlyMemory<byte>> GenerateResponse(HttpRequestParameters requestParametres);
    }
}