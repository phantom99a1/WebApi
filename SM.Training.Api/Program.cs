using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SM.Training.Utils;

namespace SM.Training.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string configurationFile = ConfigUtils.GetConfigFilePath(args);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configurationFile, optional: false, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            // Setup ConnectionString, AppSettings
            ConfigUtils.InitConfiguration(configuration);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
