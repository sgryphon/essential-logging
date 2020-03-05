using System;
using Microsoft.Extensions.Logging;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger<Program> logger = LoggerFactory
                .Create(logging => logging.AddConsole())
                .CreateLogger<Program>();
            logger.LogInformation("Hello World!");
            Console.ReadLine();
        }
    }
}
