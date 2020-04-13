using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Essential.LoggerProvider;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelloWebApp22
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
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
                .UseStartup<Startup>();
    }
}
