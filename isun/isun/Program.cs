using Microsoft.Extensions.Configuration;
using Serilog;

namespace isun
{
    public class Program
    {
        static void Main(string[] args)
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
        }
    }
}