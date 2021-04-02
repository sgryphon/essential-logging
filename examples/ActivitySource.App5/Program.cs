using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ActivitySource.App5
{
    internal class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<AppService>();
                    services.AddHostedService<LibraryActivityService>();
                });
        }

        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }
    }
}
