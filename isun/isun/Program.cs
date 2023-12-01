using isun.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net.Http.Headers;

namespace isun
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            var builder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                         .AddJsonFile("appsettings.json");

            var config = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            if (args.Length == 0)
            {
                Log.Logger.Warning("No params. Exiting..");
                return;
            }

            if (args[0] != "--cities")
            {
                Log.Logger.Warning("First param is wrong. Shoud be \"--cities\"");
                return;
            }

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://weather-api.isun.ch/api/"),
            };
            httpClient.DefaultRequestHeaders.Add("Accept", "text/plain");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("api-version", "1.0");

            var weatherService = new WeatherServiceProcessor(httpClient);
            var authResp = await weatherService.Authorize();

            if (!authResp.IsOk)
            {
                WriteLog(authResp.Error);
                return;
            }

            WriteLog("Got token: " + authResp.Object.TokenValue);

            httpClient.DefaultRequestHeaders.Add("Authorization", authResp.Object.TokenValue);

            List<string> cities = new List<string>();

            for (int i = 1; i < args.Length-1; i++)
            {
                cities.Add(args[i].Replace(",", ""));
            };


        }

        private static void WriteLog<TData>(TData data)
        {
            Log.Logger.Information($"{data}");
        }

        private static void WriteWarning<TData>(TData data)
        {
            Log.Logger.Warning($"{data}");
        }
    }
}