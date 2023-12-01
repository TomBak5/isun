namespace isun.Services
{
    public class WeatherServiceProcessor
    {
        private readonly HttpClient _httpClient;

        public WeatherServiceProcessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
