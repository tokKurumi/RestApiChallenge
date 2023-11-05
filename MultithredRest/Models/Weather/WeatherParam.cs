namespace MultithredRest.Models.Weather
{
    public class WeatherParam
    {
        public WeatherParam(string city)
        {
            City = city;
        }

        public string City { get; set; }
    }
}
