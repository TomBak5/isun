using isun.Implementations;
using isun.Services;
using isun.Tools;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net.Http.Headers;

namespace isun
{
    public class Program
    {
        private static System.Timers.Timer timer;
        private static ManualResetEvent mre = new ManualResetEvent(false);

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler((o, e) =>
            {
                mre.Set();
            });

            var builder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                         .AddJsonFile("appsettings.json");

            var config = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            Log.Logger.Information("Press Ctrl+C or Ctrl+Breal to exit..");

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

            for (int i = 1; i < args.Length - 1; i++)
            {
                cities.Add(args[i].Replace(",", ""));
            };

            timer = new System.Timers.Timer(15 * 1000);

            timer.Elapsed += async (sender, o) =>
            {
                foreach (var city in cities)
                {
                    var resp = await weatherService.GetWeather(city);

                    if (!resp.IsOk)
                        continue;

                    var cityData = resp.Object;

                    WriteLog("");
                    WriteLog("Weather forecast:");
                    WriteLog("City: " + cityData.City);
                    WriteLog("WindSpeed: " + cityData.WindSpeed);
                    WriteLog("Precipitation: " + cityData.Precipitation);
                    WriteLog("Temperature: " + cityData.Temperature);
                    WriteLog("Summary: " + cityData.Summary);

                    var db = new LightDbDataSaver(cityData);
                    DataProcessor.Save(db);
                }
            };

            timer.Start();

            await Task.Factory.StartNew(async () =>
            {
                mre.WaitOne();
            });

            timer.Stop();

            weatherService.Close();

            Log.Logger.Information("\nExiting..");

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