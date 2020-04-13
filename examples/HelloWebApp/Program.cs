using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Essential.LoggerProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloWebApp
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
