using isun.Models.RequestModels;
using isun.Models.ResponseModels;
using System.Text.Json;
using System.Text;

namespace isun.Services
{
    public class WeatherServiceProcessor
    {
        private readonly HttpClient _httpClient;

        public WeatherServiceProcessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<AuthResponse>> Authorize()
        {
            var authData = new AuthRequest()
            {
                Username = "isun",
                Password = "passwrod"
            };

            var authDataJson = JsonSerializer.Serialize(authData);
            var body = new StringContent(authDataJson, Encoding.UTF8, "application/json");

            var respMsg = await _httpClient.PostAsync("authorize", body);

            var response = new Response<AuthResponse>();
            var content = await respMsg.Content.ReadAsStringAsync();
            if (!respMsg.IsSuccessStatusCode)
            {
                response.IsOk = false;
                response.Error = JsonSerializer.Deserialize<ProblemDetails>(content);
            }
            else
            {
                response.IsOk = true;
                response.Object = JsonSerializer.Deserialize<AuthResponse>(content);
            }
            return response;
        }

        public async Task<Response<WeatherResponse>> GetWeather(string city)
        {
            var respMsg = await _httpClient.GetAsync("weathers/" + city);

            var content = await respMsg.Content.ReadAsStringAsync();
            var response = new Response<WeatherResponse>();
            if (!respMsg.IsSuccessStatusCode)
            {
                response.IsOk = false;
                response.Error = JsonSerializer.Deserialize<ProblemDetails>(content);
            }
            else
            {
                response.IsOk = true;
                response.Object = JsonSerializer.Deserialize<WeatherResponse>(content);
            }
            return response;
        }
    }
}
