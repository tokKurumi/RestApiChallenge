namespace Example.Endpoints
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Example.Models.WeatherApi;
    using Example.Services;
    using MultithredRest.Core.Attributes;
    using MultithredRest.Core.Endpoint;
    using MultithredRest.Core.HttpServer;
    using MultithredRest.Core.Result;

    [RegistrateEndpoint]
    public class WeatherEndpoint : EndpointBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherEndpoint(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string Route => @"/weather";

        public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            var body = await request.DeserializeBodyAsync<WeatherParam>(cancellationToken);

            return await OkAsync(await _weatherService.GetCityWeather(body.Postcode, body.CountryCode));
        }
    }
}