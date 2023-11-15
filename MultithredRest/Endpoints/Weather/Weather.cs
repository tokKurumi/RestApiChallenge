namespace MultithredRest.Endpoints.Weather
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MultithreadRest.Helpers;
    using MultithredRest.Core.EndpointModel;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Models.WeatherApi;
    using MultithredRest.Services;

    public class Weather : EndpointBase, IWeather
    {
        private IWeatherService _weatherService;

        public Weather(HttpMethod method, IWeatherService weatherService)
            : base(method)
        {
            _weatherService = weatherService;
        }

        public override async Task<ReadOnlyMemory<byte>> GenerateResponse(HttpRequestParameters requestParametres)
        {
            var body = await requestParametres.DeserializeBodyAsync<WeatherParam>();

            return await _weatherService.GetCityWeather(body.Postcode, body.CountryCode).SerializeJsonAsync();
        }
    }
}
