namespace MultithredRest.Endpoints.Weather
{
    using MultithredRest.Core.HttpServer;

    public interface IWeather
    {
        Task<ReadOnlyMemory<byte>> GenerateResponse(HttpRequestParameters requestParametres);
    }
}