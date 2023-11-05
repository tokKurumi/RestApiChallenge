namespace MultithredRest.Models.Weather
{
    public class WeatherCity
    {
        public WeatherCity(string city, string description)
        {
            City = city;
            WeatherDescription = description;
        }

        public string City { get; set; }

        public string WeatherDescription { get; set; }
    }
}
