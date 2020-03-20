using System;
using System.Threading.Tasks;
using Essential.LoggerProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloRollingFile
{
    public class Program
    {
        public static Random Random = new Random();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                })
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    loggingBuilder.AddRollingFile();
                    // The default configuration section is "RollingFile"; if you want
                    // a different section, you can manually configure:
                    // loggingBuilder.AddRollingFile(options =>
                    //     hostContext.Configuration.Bind("Logging:RollingFile", options));
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });

        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }
    }
}
