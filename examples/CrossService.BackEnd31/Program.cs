using Essential.LoggerProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CrossService.BackEnd31
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddElasticsearch();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
