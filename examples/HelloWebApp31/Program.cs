using System.Diagnostics;
using Essential.LoggerProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HelloWebApp31
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    //if (context.Configuration.GetSection("Logging:RollingFile").Exists())
                    {
                        loggingBuilder.AddRollingFile();
                    }
                    //if (context.Configuration.GetSection("Logging:Elasticsearch").Exists())
                    {
                        loggingBuilder.AddElasticsearch();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
