namespace MultithredRest.Endpoints
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Models.WeatherApi;
    using MultithredRest.Services;

    public class WeatherEndpoint : EndpointBase
    {
        private IWeatherService _weatherService;

        public WeatherEndpoint(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string Route => @"/weather";

        public override string HttpResponseContentType => "application/json";

        public override async Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request)
        {
            try
            {
                var body = await request.DeserializeBodyAsync<WeatherParam>();
                return await _weatherService.GetCityWeather(body.Postcode, body.CountryCode).SerializeJsonAsync();
            }
            catch (Exception ex)
            {
                return await new { Error = ex.Message }.SerializeJsonAsync();
            }
        }
    }
}